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
    private Sprite UpwardSprite, DownwardSprite, LeftSprite, RightSprite;
    private SHIPFACING currFacing, prevFacing;

    private Rigidbody2D rb = null;
    private SpriteRenderer shipSprite = null;

    private Vector2 velocity = Vector2.zero;
    private Vector2 accel = Vector2.zero;
    private float accel_rate = 10.0f;
    private float deccel_rate = 5.0f;

    private const float MAX_VEL = 10.0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shipSprite = GetComponent<SpriteRenderer>();
        prevFacing = currFacing = SHIPFACING.LEFT;

        UpdateUI(ownerName, ownerName);

    }

    public override void OnStartAuthority()
    {
        Debug.Log("BattleShip: Taken Authority Over BattleShip");
      
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

    [Command]
    private void SyncShipStatus(bool show)
    {
        isVisible = show;
    }

    [Command]
    private void SyncShipSprite(int facing)
    {
        SetShipSprite(facing);
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
}

enum SHIPFACING
{
    UP,
    DOWN,
    LEFT,
    RIGHT,

    NUM_TOTAL
}

