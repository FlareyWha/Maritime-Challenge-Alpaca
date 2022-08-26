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
    [SyncVar (hook = nameof(OnUsernameSet))]
    private string username = "Unset";

    private PlayerUI playerUI = null;

   
    public override void OnStartLocalPlayer()
    {
        // GO Inits
        playerUI = GetComponent<PlayerUI>();

        // Set Local Player if this is yours
        if (isLocalPlayer)
        {
            PlayerData.MyPlayer = this;
            username = PlayerData.Name;
        }

        // Player UI Inits for New Players (No Callback On Set)
        playerUI.SetDisplayName(username);

        // Init Player to SpawnPos

        // Attach Camera
        UIManager.Instance.Camera.SetFollowTarget(gameObject);

    }

    void OnUsernameSet(string prev_name, string new_name)
    {
        playerUI.SetDisplayName(new_name);
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
