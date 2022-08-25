using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


// Client - Function runs only on Clients
// ClientRpc - Function Called by Server, Runs it on the same GO on all Clients
// Command - When Function is Called, It is Run on the Server

public class PlayerCommands : NetworkBehaviour
{

    public void SendChatMessage(string message)
    {
        Debug.Log("Sending A Message..");
        SendMessageToServer(ChatManager.Instance.GetChatType(), PlayerData.MyPlayer, message);
    }

    [Command]
    void SendMessageToServer(CHAT_TYPE type, Player player, string message)
    {
        Debug.Log("Sending Message to Server");
        UpdateChatLog(type, player, message);
    }

    [ClientRpc]
    void UpdateChatLog(CHAT_TYPE chat_type, Player player, string message)
    {
        Debug.Log("Received RPC from Server, Updating Chat Log...");

       if (player == null)
        {
            Debug.Log("Player was NULL!!");
            return;
        }
        ChatManager.Instance.UpdateChatLog(chat_type, player.GetUsername(), message);
    }
}
