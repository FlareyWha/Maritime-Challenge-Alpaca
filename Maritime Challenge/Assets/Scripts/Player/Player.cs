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

    private PlayerUI playerUI = null;


    public override void OnStartAuthority()
    {

        // Init Player to SpawnPos

        // Set My Player
        PlayerData.MyPlayer = this;
        // Init Synced Player Vars
        SetUsername(PlayerData.Name);
        Debug.Log("Setting Player Name.." + username);

    }
    public override void OnStartClient()
    {
        EnsureInits();

        // Player UI Inits for New Players (No Callback On Set)
        playerUI.SetDisplayName(username);
        Debug.Log("Initting Player Name..." + username);


        // Attach Camera
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
    void SetUsername(string name)
    {
        username = name;
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

    public int GetGuildID()
    {
        return guildID;
    }

    public int GetUID()
    {
        return UID;
    }
}
