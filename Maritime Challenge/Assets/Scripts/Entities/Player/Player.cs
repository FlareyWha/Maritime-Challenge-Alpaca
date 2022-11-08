using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Mirror;
using UnityEngine.Networking;

public class Player : BaseEntity
{
    [SerializeField]
    private GameObject BattleShipPrefab;
    [SerializeField]
    private SpriteRenderer BodyReferenceSprite;
    public SpriteRenderer RefSprite { get { return BodyReferenceSprite; } }

    [SerializeField]
    private SpriteRenderer[] PlayerSprites;
    private int[] defaultSortingOrder = new int[(int)BODY_PART_TYPE.NUM_TOTAL];


    [SyncVar]
    private int UID = 0;
    [SyncVar]
    private int guildID = 0;
    [SyncVar(hook = nameof(OnUsernameChanged))]
    private string username = "";
    [SyncVar]
    private string bio = "";
    [SyncVar]
    private int level = 0;
    [SyncVar]
    private int countryID = 0;
    [SyncVar]
    private int titleID = 0;

    [SyncVar]
    private bool isVisible = true;
    [SyncVar(hook = nameof(OnShipSet))]
    private GameObject LinkedBattleshipGO = null;
    public GameObject BattleshipGO { get { return LinkedBattleshipGO; } }

    private PlayerUI playerUI = null;
    private PlayerAnimationsManager playerAnimationsManager = null;
    private Battleship LinkedBattleship = null;


   
    private void Awake()
    {
        gameObject.SetActive(isVisible);
        for (int i = 0; i < PlayerSprites.Length; i++)
        {
            defaultSortingOrder[i] = PlayerSprites[i].sortingOrder;
        }
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            for (int i = 0; i < defaultSortingOrder.Length; i++)
            {
                defaultSortingOrder[i] += 3;
            }
        }
    }


    public override void OnStartLocalPlayer()
    {
        // Set My Player
        PlayerData.MyPlayer = this;
        // Init Synced Player Vars
        CallSetDetails();

        PlayerData.OnPlayerDataUpdated += CallSetDetails;

        // Set Position
        transform.position = SceneManager.StartWorldHubSpawnPos;
       
        //Init My BattleShip
        if (LinkedBattleshipGO == null)
            SpawnBattleShip();

    }

    public override void OnStopLocalPlayer()
    {
        PlayerData.OnPlayerDataUpdated -= CallSetDetails;
    }

    private void CallSetDetails()
    {
        StartCoroutine(SetDetails());
    }

    IEnumerator SetDetails()
    {
        while (PlayerData.EquippedCosmeticsList.Count == 0)
            yield return null;

      
        SetDetails(PlayerData.UID, PlayerData.Name, PlayerData.Biography, PlayerData.CurrentTitleID, PlayerData.GuildID, PlayerData.Country, PlayerData.CurrLevel);
    }
    public override void OnStartClient()
    {
        EnsureInits();

        // Player UI Inits for New Players (No Callback On Set)
        playerUI.SetDisplayName(username);
        Debug.Log("Initting Player Name..." + username);


        // Attach Camera
        if (isLocalPlayer)
            UIManager.Instance.Camera.SetFollowTarget(gameObject);


        base.OnStartClient();
    }

    private void EnsureInits()
    {
        // GO Inits
        if (playerUI == null)
            playerUI = GetComponent<PlayerUI>();
        if (playerAnimationsManager == null)
            playerAnimationsManager = GetComponent<PlayerAnimationsManager>();
    }

    [Command]
    void SetDetails(int id, string name, string bio, int title_id, int guild_id, int country_id, int level)
    {
        UID = id;
        username = name;
        this.bio = bio;
        titleID = title_id;
        guildID = guild_id;
        countryID = country_id;
        this.level = level;

        isVisible = true;
        LinkedBattleshipGO = null;
    }

    [Command]
    void SpawnBattleShip()
    {
        GameObject ship = Instantiate(BattleShipPrefab);
        SceneManager.Instance.MoveGameObjectToScene(ship, "WorldHubScene");
        NetworkServer.Spawn(ship, connectionToClient);
        LinkedBattleshipGO = ship;

        Battleship bs = ship.GetComponent<Battleship>();
        bs.ServerInits();
        bs.SetOwner(connectionToClient.identity.gameObject.GetComponent<Player>());

    }

    private void Update()
    {
        if (InputManager.InputActions.Main.Tap.WasPressedThisFrame() && SpriteHandler.IsWithinSprite(transform.position, BodyReferenceSprite))
            OnPlayerClicked();

    }

    private void OnPlayerClicked()
    {
        Debug.Log("Player Entity Click Called");
        if (!isLocalPlayer)
            playerUI.OpenInteractPanel();
    }

 
    public void SummonBattleShip(Dock dock)
    {
        if (LinkedBattleship == null)
        {
            Debug.Log("Player does not have a Linked BatleShip!!");
            return;
        }

        LinkedBattleship.Summon(dock.GetRefShipTransform());
        PlayerFollowCamera.Instance.SetFollowTarget(LinkedBattleship.gameObject);
        PlayerFollowCamera.Instance.ZoomCameraInOut(30);
        SyncPlayerVisibility(false);
    }

    public void DockShip(Dock dock)
    {
        if (LinkedBattleship == null)
        {
            Debug.Log("Player does not have a Linked BatleShip!!");
            return;
        }

        LinkedBattleship.Dock();
        Transform playerT = dock.GetRefPlayerTransform();
        transform.position = playerT.position;
        transform.rotation = playerT.rotation;
        PlayerFollowCamera.Instance.SetFollowTarget(gameObject);
        PlayerFollowCamera.Instance.ResetCameraZoom();
        SyncPlayerVisibility(true);
    }

    [Command]
    private void SyncPlayerVisibility(bool show)
    {
        SetPlayerVisibility(show);
        isVisible = show;
    }

    [ClientRpc]
    private void SetPlayerVisibility(bool show)
    {
        gameObject.SetActive(show);
    }

   
    // ============ SYNCVAR CALLBACKS ====================

    void OnUsernameChanged(string prev_name, string new_name)
    {
        EnsureInits();

        playerUI.SetDisplayName(username);
        Debug.Log("Name Received, " + username);
    }

    void OnShipSet(GameObject old, GameObject newGO)
    {
        if (LinkedBattleshipGO == null)
        {
            LinkedBattleship = null;
            return;
        }

        LinkedBattleship = newGO.GetComponent<Battleship>();
        LinkedBattleship.SetOwner(this);
        LinkedBattleshipGO.SetActive(false);
        if (isLocalPlayer)
        {
            LinkedBattleship.InitShip(username);
        }
         OnEntityHPChanged += LinkedBattleship.SetHP;
    }

    public void SetOrderInLayer(int what)
    {
        for (int i = 0; i < PlayerSprites.Length; i++)
        {
            PlayerSprites[i].sortingOrder = defaultSortingOrder[i] + what;
        }
    }

    // ================= GETTERS =====================
    public string GetUsername()
    {
        return username;
    }
    public string GetBio()
    {
        return bio;
    }
    public int GetGuildID()
    {
        return guildID;
    }
    public int GetUID()
    {
        return UID;
    }
    public int GetTitleID()
    {
        return titleID;
    }
    public int GetLevel()
    {
        return level;
    }
    public int GetCountryID()
    {
        return countryID;
    }

    public Battleship GetBattleShip()
    {
        return LinkedBattleship;
    }

}
