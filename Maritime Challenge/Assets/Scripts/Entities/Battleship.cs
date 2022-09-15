using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Battleship : NetworkBehaviour
{

    [SyncVar(hook=nameof(OnShipStatusChanged))]
    private bool isVisible = false;
    [SyncVar(hook =nameof(UpdateUI))]
    private string ownerName = "";

    [SerializeField]
    private Text OwnerNameText;
    [SerializeField]
    private Image OwnerNameBG;
    [SerializeField]
    private Image HPFill;
    [SerializeField]
    private Sprite UpwardSprite, DownwardSprite, LeftSprite, RightSprite;
    private SHIPFACING currFacing, prevFacing;

    [SerializeField]
    private GameObject CannonBallPrefab;

    private Rigidbody2D rb = null;
    private SpriteRenderer shipSprite = null;
    private BaseEnemy currTarget = null;
    private Player ownerPlayer = null;

    private Vector2 velocity = Vector2.zero;
    private Vector2 accel = Vector2.zero;
    private float accel_rate = 20.0f;
    private float deccel_rate = 10.0f;
    private const float MAX_VEL = 10.0f;

    private const float TARGET_RANGE = 30.0f;

    private const float FIRE_INTERVAL = 2.0f;
    private float fire_timer = 0.0f;

    [SerializeField]
    private GameObject selectedTargetUI;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shipSprite = GetComponent<SpriteRenderer>();
        prevFacing = currFacing = SHIPFACING.LEFT;

        UpdateUI(ownerName, ownerName);
        gameObject.SetActive(false);
    }

    public override void OnStartAuthority()
    {
        Debug.Log("BattleShip: Taken Authority Over BattleShip");
    }

    public void SetOwner(Player player)
    {
        ownerPlayer = player;
    }

    [Server]
    public void ServerInits()
    {
        isVisible = false;
        ownerName = "";
    }
  
    [Command]
    public void InitShip(string name)
    {
        isVisible = false;
        ownerName = name;

    }
    private void UpdateUI(string oldname, string newname)
    { 
        OwnerNameText.text = ownerName;
        UIManager.SetWidthByTextWidth(OwnerNameBG.gameObject, OwnerNameText);
    }
   

    void Update()
    {
        if (!hasAuthority)
            return;

        // Update Ship Movement
        // Update Accel if Joystick is Moved
        if (UIManager.Instance.Joystick.GetDirection() != Vector2.zero)
        {
            accel = UIManager.Instance.Joystick.GetDirection();
            velocity += accel * accel_rate * Time.deltaTime;
        }
        // Stop Ship if coming to a stop  (curr dir diff from last accel dir) - so it doesn't go backwards from deccel
        if (velocity.magnitude != 0 && 
            Vector2.Dot(accel, velocity) < 0)
        {
            velocity = Vector2.zero;
        }

        // Apply Decceleration if ship is moving
        if (velocity.magnitude != 0)
            velocity -= velocity.normalized * deccel_rate * Time.deltaTime;

        // Clamp values wihtin Max Limit
        velocity.x = Mathf.Clamp(velocity.x, -MAX_VEL, MAX_VEL);
        velocity.y = Mathf.Clamp(velocity.y, -MAX_VEL, MAX_VEL);

        // Move Ship By Velocity
        rb.position += velocity * Time.deltaTime;

        // Update Ship Sprite
        prevFacing = currFacing;
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            if (velocity.x > 0)
                currFacing = SHIPFACING.RIGHT;
            else
                currFacing = SHIPFACING.LEFT;
        }
        else
        {
            if (velocity.y > 0)
                currFacing = SHIPFACING.UP;
            else
                currFacing = SHIPFACING.DOWN;
        }
        if (prevFacing != currFacing)
            SyncShipSprite((int)currFacing);

        // Update Ship Attack
        if (currTarget != null)
        {
            // Check for Stop Attack when out of range
            Vector3 dis = currTarget.transform.position - transform.position;
            dis.z = 0;
            if (dis.magnitude > TARGET_RANGE)
            {
                currTarget = null; 
                selectedTargetUI.transform.position = transform.position + selectedTargetUI.transform.localPosition;
                selectedTargetUI.transform.SetParent(transform);
                selectedTargetUI.gameObject.SetActive(false);
            }

            // Fire Cannonball
            fire_timer -= Time.deltaTime;
            if (fire_timer <= 0.0f)
            {
                FireCannon();
                fire_timer = FIRE_INTERVAL;
            }

        }

    }

    private void FireCannon()
    {
        LaunchCannonBall(currTarget.gameObject, PlayerData.activeSubScene);
    }

    [Command]
    private void LaunchCannonBall(GameObject target, string currSceneName)
    {
        GameObject ball = Instantiate(CannonBallPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(ball);

        CannonBall cannonBall = ball.GetComponent<CannonBall>();
        cannonBall.Init(target);

        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(ball,
            UnityEngine.SceneManagement.SceneManager.GetSceneByName(currSceneName));
    }

    public bool SetTarget(BaseEnemy enemy)
    {
        Debug.Log("Set Target To: " + enemy.name);

        Vector3 dis = enemy.transform.position - transform.position;
        dis.z = 0;
        if (dis.magnitude > TARGET_RANGE)
            return false;

        currTarget = enemy;
        selectedTargetUI.transform.position = enemy.transform.position + selectedTargetUI.transform.localPosition;
        selectedTargetUI.transform.SetParent(enemy.transform);
        selectedTargetUI.gameObject.SetActive(true);

        fire_timer = 0.0f;
        return true;
    }


    public void Dock()
    {
        Debug.Log("Ship Docked");
        SyncShipStatus(false);
    }

    public void Summon(Transform refTransform)
    {
        Debug.Log("Ship Summoned");
        transform.position = refTransform.position;
        transform.rotation = refTransform.rotation;
        currFacing = SHIPFACING.LEFT;
        SyncShipSprite((int)currFacing);
        SyncShipStatus(true);
    }

    public void SetHP(int oldhp, int newhp)
    {
        HPFill.fillAmount = (float)oldhp / newhp;
    }

    [Command]
    private void SyncShipStatus(bool show)
    {
        isVisible = show;
        gameObject.SetActive(isVisible);
    }

    [Command]
    private void SyncShipSprite(int facing)
    {
        SetShipSprite(facing);
        switch ((SHIPFACING)facing)
        {
            case SHIPFACING.LEFT:
                shipSprite.sprite = LeftSprite;
                break;
            case SHIPFACING.RIGHT:
                shipSprite.sprite = RightSprite;
                break;
            case SHIPFACING.UP:
                shipSprite.sprite = UpwardSprite;
                break;
            case SHIPFACING.DOWN:
                shipSprite.sprite = DownwardSprite;
                break;
        }
    }

    public void OnShipStatusChanged(bool old, bool show)
    {
        Debug.Log("Ship Visibility Callback: Set To " + isVisible);
        gameObject.SetActive(isVisible);
    }

    [ClientRpc]
    private void SetShipSprite(int facing)
    {
        switch ((SHIPFACING)facing)
        {
            case SHIPFACING.LEFT:
                shipSprite.sprite = LeftSprite;
                break;
            case SHIPFACING.RIGHT:
                shipSprite.sprite = RightSprite;
                break;
            case SHIPFACING.UP:
                shipSprite.sprite = UpwardSprite;
                break;
            case SHIPFACING.DOWN:
                shipSprite.sprite = DownwardSprite;
                break;
        }
    }

    public Player GetOwner()
    {
        return ownerPlayer;
    }

}

enum SHIPFACING
{
    UP,
    DOWN,
    LEFT,
    RIGHT,

    NUM_TOTAL
}

