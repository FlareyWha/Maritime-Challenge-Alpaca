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
    private Transform TurretHoleRef_Up, TurretHoleRef_Down, TurretHoleRef_Left, TurretHoleRef_Right;
    private SHIP_FACING currFacing, prevFacing;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private BattleShipAnimatorHandler animatorhandler;
    [SyncVar]
    private bool anim_isMoving;
    [SyncVar]
    private Vector2 anim_dir;

    private Rigidbody2D rb = null;
    private SpriteRenderer shipSprite = null;
    private BaseEnemy currTarget = null;
    private Player ownerPlayer = null;

    private Vector2 velocity = Vector2.zero;
    private float theta = 0.0f;
    private float target_theta = 0.0f;
    private float delta_theta = 0.0f;
    private Vector2 direction = Vector2.zero;
    private float angular_velocity = 0.0f;
    private float angular_accel_rate = 50.0f;
    private float angular_deccel_rate = 5.0f;
    private float accel_rate = 20.0f;
    private float deccel_rate = 10.0f;
    private const float MAX_VEL = 13.0f;
    private const float MAX_ANGULAR_VEL = 40.0f;

    private const float TARGET_RANGE = 30.0f;

    private float FIRE_INTERVAL = 2.0f;
    private float fire_timer = 0.0f;

    [SerializeField]
    private GameObject selectedTargetUI;

    [SerializeField]
    private AudioSource launchAudioSource;

    private void Start()
    {
        Debug.Log("BattleShip Set to " + isVisible);
        gameObject.SetActive(isVisible);
    }
    public override void OnStartServer()
    {
        anim_isMoving = false;
        anim_dir = Vector2.zero;
    }

    public override void OnStartClient()
    {
        animator.SetBool("IsMoving", anim_isMoving);
        animator.SetFloat("DirX", anim_dir.x);
        animator.SetFloat("DirY", anim_dir.y);
    }
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
        UIManager.SetWidthByTextWidth(OwnerNameBG.gameObject, OwnerNameText, 5);
    }


    void Update()
    {
        if (!hasAuthority)
            return;

        // Update Ship Movement
        // Update Accel if Joystick is Moved
        if (UIManager.Instance.Joystick.GetDirection() != Vector2.zero)
        {
            // Update Angular Vel
            Vector2 joystickDir = UIManager.Instance.Joystick.GetDirection();
            target_theta = Vector2.SignedAngle(Vector2.up, joystickDir);
            delta_theta = Vector2.SignedAngle(velocity, joystickDir);

            angular_velocity += delta_theta * angular_accel_rate * Time.deltaTime;

            // update direction var for later use/ref
            direction = Quaternion.Euler(0, 0, theta) * Vector2.up;
            // Update vel speed
            float speed = velocity.magnitude + accel_rate * Time.deltaTime;

            // Apply new speed and dir to Vel
            velocity = direction * speed;
        }

        // Update Theta
        theta += angular_velocity * Time.deltaTime;
        // Check if Overshot Target Dir
        if (theta > target_theta && delta_theta > 0
            || theta < target_theta && delta_theta < 0)
            angular_velocity = 0;


        // Apply Decceleration if ship is moving
        if (velocity.magnitude != 0)
            velocity -= velocity.normalized * deccel_rate * Time.deltaTime;
        // Deccelerate Angular Velocity
        if (angular_velocity != 0.0f)
        {
            if (angular_velocity > 0.0f)
            {
                angular_velocity -= angular_deccel_rate * Time.deltaTime;
                if (angular_velocity < 0.0f)
                    angular_velocity = 0.0f;
            }
            else
            {
                angular_velocity += angular_deccel_rate * Time.deltaTime;
                if (angular_velocity > 0.0f)
                    angular_velocity = 0.0f;
            }
        }

        // Stop Ship if coming to a stop  (curr dir diff from last accel dir) - so it doesn't go backwards from deccel
        if (velocity.magnitude != 0 && Vector2.Dot(direction, velocity) < 0)
            velocity = Vector2.zero;

        // Clamp values wihtin Max Limit
        velocity.x = Mathf.Clamp(velocity.x, -MAX_VEL, MAX_VEL);
        velocity.y = Mathf.Clamp(velocity.y, -MAX_VEL, MAX_VEL);
        angular_velocity = Mathf.Clamp(angular_velocity, -MAX_ANGULAR_VEL, MAX_ANGULAR_VEL);

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
        rb.rotation = theta;

        // Update Animations
        SendUpdateAnimatorMoving(velocity.magnitude != 0);

        Vector2 dirrr = Quaternion.Euler(0, 0, theta) * Vector2.up;
        SendUpdateAnimatorDir(dirrr.x, dirrr.y);


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
        LaunchCannonBall(currTarget.gameObject, GetTurretHoleRefPos(), new Vector3(-Mathf.Sin(rad), Mathf.Cos(rad), 0), PlayerData.activeSubScene);
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

        PlayLaunchCannonballSFX(currSceneName, spawnPos);
    }

    [ClientRpc]
    private void PlayLaunchCannonballSFX(string sceneName, Vector3 pos)
    {
        if (PlayerData.activeSubScene != sceneName)
            return;

        // Play SFX
        launchAudioSource.Play();
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
        SyncShipFacing((int)currFacing);
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
        if (shipInfo == null)
            return;

        animatorhandler.SetAnimations(id);
    }
    [Command]
    private void SendUpdateAnimatorMoving(bool moving)
    {
        anim_isMoving = moving;
        UpdateAnimatorWalkBool(moving);
    }

    [Command]
    private void SendUpdateAnimatorDir(float x, float y)
    {
        anim_dir.Set(x, y);
        UpdateAnimatorDirValues(x, y);
    }

    [ClientRpc]
    private void UpdateAnimatorWalkBool(bool moving)
    {
        animator.SetBool("IsMoving", moving);

    }


    [ClientRpc]
    private void UpdateAnimatorDirValues(float x, float y)
    {
        animator.SetFloat("DirX", x);
        animator.SetFloat("DirY", y);

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

