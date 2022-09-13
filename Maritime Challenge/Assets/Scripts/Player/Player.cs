using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mirror;
using UnityEngine.Networking;

public class Player : BaseEntity
{
    [SerializeField]
    private GameObject BattleShipPrefab;


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

    private Battleship LinkedBattleship = null;


    private PlayerUI playerUI = null;

    private void Start()
    {
        gameObject.SetActive(isVisible);
        base.InitSpriteSize();
    }

    public override void OnStartLocalPlayer()

    {
        // Init Player to SpawnPos

        // Set My Player
        PlayerData.MyPlayer = this;
        // Init Synced Player Vars
        SetDetails(PlayerData.UID, PlayerData.Name, PlayerData.Biography, PlayerData.CurrentTitleID, PlayerData.GuildID, PlayerData.Country, PlayerData.CurrLevel);
        Debug.Log("Setting Player Name.." + username);

        PlayerData.OnPlayerDataUpdated += SetDetails;

        //Init My BattleShip
        SpawnBattleShip();


        //DontDestroyOnLoad(this);
        //transform.position = new Vector3(-10, -10, 0);
    }


    private void SetDetails()
    {
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
        NetworkServer.Spawn(ship, connectionToClient);
        LinkedBattleshipGO = ship;

        Battleship bs = ship.GetComponent<Battleship>();
        bs.ServerInits();

    }

    private void Update()
    {
        base.CheckForEntityClick();
    }
    public override void OnEntityClicked()
    {
        Debug.Log("Player Entity Click Called");
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
        if (isLocalPlayer)
            LinkedBattleship.InitShip(username);
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

    // ================ DATABASE ======================
    public void UpdateXPLevels(int xpGained)
    {
        int currXP = PlayerData.CurrXP;
        int currLevel = PlayerData.CurrLevel;
        bool finishedLevelingUp = false;

        //Add up all the XP
        currXP += xpGained;

        do
        {
            //Get the xp requirement
            int xpRequirement = GameSettings.GetEXPRequirement(currLevel);

            //Increase level if currXP meets the xpRequirement
            if (currXP >= xpRequirement)
            {
                currLevel++;
                currXP -= xpRequirement;
            }
            else
                finishedLevelingUp = true;
        }
        while (!finishedLevelingUp);

        //Update player data values
        PlayerData.CurrXP = currXP;
        PlayerData.CurrLevel = currLevel;

        //Update database
        StartCoroutine(StartUpdateXPLevels());
    }

    IEnumerator StartUpdateXPLevels()
    {
        string url = ServerDataManager.URL_updateAccountXPLevels;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("iLevel", PlayerData.CurrLevel);
        form.AddField("iXP", PlayerData.CurrXP);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                Debug.Log(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

}
