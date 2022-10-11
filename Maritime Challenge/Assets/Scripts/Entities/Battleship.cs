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
    [SerializeField]
    private Transform TurretHoleRef_Up, TurretHoleRef_Down, TurretHoleRef_Left, TurretHoleRef_Right;
    private SHIP_FACING currFacing, prevFacing;

 //   [SerializeField]
  //  private GameObject CannonBallPrefab;

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

    private float FIRE_INTERVAL = 2.0f;
    private float fire_timer = 0.0f;

    [SerializeField]
    private GameObject selectedTargetUI;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shipSprite = GetComponent<SpriteRenderer>();
        prevFacing = currFacing = SHIP_FACING.LEFT;

        UpdateUI(ownerName, ownerName);
        gameObject.SetActive(false);
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        SyncBattleshipSprites(PlayerData.CurrentBattleship);
    }

    public void SetOwner(Player player)
    {
        ownerPlayer = player;
        FIRE_INTERVAL = player.ATKSPD;
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
                currFacing = SHIP_FACING.RIGHT;
            else
                currFacing = SHIP_FACING.LEFT;
        }
        else
        {
            if (velocity.y > 0)
                currFacing = SHIP_FACING.UP;
            else
                currFacing = SHIP_FACING.DOWN;
        }
        if (prevFacing != currFacing)
            SyncShipFacing((int)currFacing);

        // Update Rotation
        float theta = Vector2.SignedAngle(Vector2.up, velocity);
        rb.rotation = theta;

        // Update Ship Attack
        if (currTarget != null)
        {   
            // Check for Disable (?)
            if (!currTarget.gameObject.activeSelf)
            {
                currTarget = null;
                UpdateSelectedTargetUI();
                return;
            }


            // Check for Stop Attack when out of range
            Vector3 dis = currTarget.transform.position - transform.position;
            dis.z = 0;
            if (dis.magnitude > TARGET_RANGE)
            {
                currTarget.OnEntityDied -= OnTargetDiedCallback;
                currTarget = null;
                UpdateSelectedTargetUI();
            }

            // Fire Cannonball
            fire_timer -= Time.deltaTime;
            if (fire_timer <= 0.0f)
            {
                FireCannon(rb.rotation);
                fire_timer = FIRE_INTERVAL;
            }

        }

    }

    private void UpdateSelectedTargetUI()
    {
        if (currTarget == null)
        {
            selectedTargetUI.transform.position = transform.position + selectedTargetUI.transform.localPosition;
            selectedTargetUI.transform.SetParent(transform);
            selectedTargetUI.gameObject.SetActive(false);
        }
        else
        {
            selectedTargetUI.transform.position = currTarget.transform.position + selectedTargetUI.transform.localPosition;
            selectedTargetUI.transform.SetParent(currTarget.transform);
            selectedTargetUI.gameObject.SetActive(true);
        }
    }

    private void FireCannon(float theta)
    {
        float rad = Mathf.Deg2Rad * theta;
        LaunchCannonBall(currTarget.gameObject, GetTurretHoleRefPos(), new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0), PlayerData.activeSubScene);
    }

    private Vector3 GetTurretHoleRefPos()
    {
        switch (currFacing)
        {
            case SHIP_FACING.LEFT:
                return TurretHoleRef_Left.position;
            case SHIP_FACING.RIGHT:
                return TurretHoleRef_Right.position;
            case SHIP_FACING.UP:
                return TurretHoleRef_Up.position;
            case SHIP_FACING.DOWN:
                return TurretHoleRef_Down.position;
            default:
                return Vector3.zero;
        }
    }

    [Command]
    private void LaunchCannonBall(GameObject target, Vector3 spawnPos, Vector3 shipDir, string currSceneName)
    {
      
        CannonBall ball = ProjectileManager.Instance.GetActiveCannonBall();
        ball.transform.position = spawnPos;
        ball.Activate();
        SceneManager.Instance.MoveGameObjectToScene(ball.gameObject, currSceneName);

        ball.Init(target, shipDir, ownerPlayer);
    }

    private void OnTargetDiedCallback()
    {
        currTarget = null;
        UpdateSelectedTargetUI();
    }

    public bool SetTarget(BaseEnemy enemy)
    {
        if (currTarget == enemy)
        {
            currTarget = null;
            UpdateSelectedTargetUI();
            return false;
        }

        Vector3 dis = enemy.transform.position - transform.position;
        dis.z = 0;
        if (dis.magnitude > TARGET_RANGE)
            return false;

        currTarget = enemy;
        currTarget.OnEntityDied += OnTargetDiedCallback;
        UpdateSelectedTargetUI();

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
        currFacing = SHIP_FACING.LEFT;
        //SyncShipSprite((int)currFacing);
        SyncShipStatus(true);
    }

    public void SetHP(int oldhp, int newhp)
    {
        HPFill.fillAmount = (float)newhp / ownerPlayer.MaxHP;
    }

    [Command]
    private void SyncShipStatus(bool show)
    {
        isVisible = show;
        gameObject.SetActive(isVisible);
    }

    [Command]
    private void SyncShipFacing(int facing)
    {
        SetShipFacing(facing);
        currFacing = (SHIP_FACING)facing;
        UpdateShipSpriteBaseOnFacing();
    }



    public void OnShipStatusChanged(bool old, bool show)
    {
        Debug.Log("Ship Visibility Callback: Set To " + isVisible);
        gameObject.SetActive(isVisible);
    }

    [ClientRpc]
    private void SetShipFacing(int facing)
    {
        currFacing = (SHIP_FACING)facing;
        UpdateShipSpriteBaseOnFacing();
    }

    private void UpdateShipSpriteBaseOnFacing()
    {
        switch (currFacing)
        {
            case SHIP_FACING.LEFT:
                shipSprite.sprite = LeftSprite;
                break;
            case SHIP_FACING.RIGHT:
                shipSprite.sprite = RightSprite;
                break;
            case SHIP_FACING.UP:
                shipSprite.sprite = UpwardSprite;
                break;
            case SHIP_FACING.DOWN:
                shipSprite.sprite = DownwardSprite;
                break;
        }
    }

    [Command]
    public void SyncBattleshipSprites(int id)
    {
        ChangeBattleShip(id);
    }

    [ClientRpc]
    public void ChangeBattleShip(int id)
    {
        BattleshipInfo shipInfo = PlayerData.FindBattleshipInfoByID(id);
        LeftSprite = shipInfo.BattleshipData.LeftSprite;
        RightSprite = shipInfo.BattleshipData.RightSprite;
        UpwardSprite = shipInfo.BattleshipData.UpwardSprite;
        DownwardSprite = shipInfo.BattleshipData.DownwardSprite;

        UpdateShipSpriteBaseOnFacing();
    }

    public Player GetOwner()
    {
        return ownerPlayer;
    }

}

enum SHIP_FACING
{
    UP,
    DOWN,
    LEFT,
    RIGHT,

    NUM_TOTAL
}

