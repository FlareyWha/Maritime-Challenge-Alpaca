using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{

    private int guildID = 0;
    private string username = "Unset";

    void Start()
    {
        // TBC! - Init from Player Data when player data is set
        //  PlayerDisplayName.text = PlayerData.Name;
        // PlayerID = PlayerData.ID;

    }
    public override void OnStartLocalPlayer()
    {
        // Set Local Player if this is yours
        if (isLocalPlayer)
            PlayerData.MyPlayer = this;

        // Init Player to SpawnPos

        // Attach Camera
        UIManager.Instance.Camera.SetFollowTarget(gameObject);

    }

    public string GetUsername()
    {
        return username;
    }

    public int GetGuildID()
    {
        return guildID;
    }
}
