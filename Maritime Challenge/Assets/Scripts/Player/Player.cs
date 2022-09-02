using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private int UID = 0;
    [SyncVar]
    private int guildID = 0;
    [SyncVar(hook = nameof(SyncUsername))]
    private string username = "";
    [SyncVar]
    private string bio = "";
    [SyncVar]
    private int level = 0;
    [SyncVar]
    private int countryID = 0;
    [SyncVar]
    private int titleID = 0;

    private PlayerUI playerUI = null;


    public override void OnStartAuthority()
    {
        // Init Player to SpawnPos

        // Set My Player
        PlayerData.MyPlayer = this;
        // Init Synced Player Vars
        SetDetails(PlayerData.UID, PlayerData.Name, PlayerData.Biography, PlayerData.CurrentTitleID, PlayerData.GuildID, PlayerData.Country, PlayerData.CurrLevel);
        Debug.Log("Setting Player Name.." + username);

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
    }

    void SyncUsername(string prev_name, string new_name)
    {
        EnsureInits();

        username = new_name;
        playerUI.SetDisplayName(new_name);
        Debug.Log("Name Received, " + new_name);
    }

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

}
