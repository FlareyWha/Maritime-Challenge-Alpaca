using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;


// Client - Function runs only on Clients
// ClientRpc - Function Called by Server, Runs it on the same GO on all Clients
// Command - When Function is Called, It is Run on the Server

public class PlayerCommands : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        PlayerData.CommandsHandler = this;
    }

    public void SendChatMessage(string message)
    {
        Debug.Log("Sending A Message..");
        SendMessageToServer(ChatManager.Instance.GetChatType(), PlayerData.MyPlayer, message);
    }

    [Command]
    private void SendMessageToServer(CHAT_TYPE type, Player player, string message)
    {
        Debug.Log("Sending Message to Server");
        UpdateChatLog(type, player, message);
    }

    [ClientRpc]
    private void UpdateChatLog(CHAT_TYPE chat_type, Player player, string message)
    {
        Debug.Log("Received RPC from Server, Updating Chat Log...");

        if (player == null)
        {
            Debug.Log("Player was NULL!!");
            return;
        }

        if (chat_type == CHAT_TYPE.GUILD && player.GetGuildID() != PlayerData.GuildID)
            return;

        ChatManager.Instance.UpdateChatLog(chat_type, player.GetUsername(), message);
        PlayerUI playerUI = player.gameObject.GetComponent<PlayerUI>();
        playerUI.AddChatBubble(message);
    }

    public void SendDeletedFriendRequestEvent(int sender_id, int rec_id)
    {
        SendDeletedFriendRequestEventToServer(sender_id, rec_id);
    }

    [Command]
    private void SendDeletedFriendRequestEventToServer(int senderID, int recID)
    {
        DeleteFriendRequest(senderID, recID);
    }

    [ClientRpc]
    private void DeleteFriendRequest(int senderID, int recID)
    {
        Debug.Log("Received Deleted Friend Request Event from Player " + senderID + " to Player " + recID);

        if (senderID == PlayerData.UID)
        {
            PlayerData.SentFriendRequestList.Remove(recID);
            FriendRequestHandler.InvokeFriendRequestDeletedEvent(senderID, recID);
        }
        else if (recID == PlayerData.UID)
        {
            PlayerData.ReceivedFriendRequestList.Remove(senderID);
            FriendRequestHandler.InvokeFriendRequestDeletedEvent(senderID, recID);
        }
    }

    public void SendFriendRequestEvent(int sender_id, int rec_id)
    {
        SendFriendRequestEventToServer(sender_id, rec_id);
    }

    [Command]
    private void SendFriendRequestEventToServer(int senderID, int recID)
    {
        SendSentFriendRequest(senderID, recID);
    }

    [ClientRpc]
    private void SendSentFriendRequest(int senderID, int recID)
    {

        Debug.Log("Received Sent Friend Request Event from Player " + senderID + " to Player " + recID);

        if (senderID == PlayerData.UID)
        {
            PlayerData.SentFriendRequestList.Add(recID);
            FriendRequestHandler.InvokeFriendRequestSentEvent(senderID, recID);
        }
        else if (recID == PlayerData.UID)
        {
            PlayerData.ReceivedFriendRequestList.Add(senderID);
            FriendRequestHandler.InvokeFriendRequestSentEvent(senderID, recID);
        }

    }

    public void SendFriendAddedEvent(int recID)
    {
        Debug.Log("COMMAND: Sending Friend Added Event for: " + recID +  "from " + PlayerData.UID);
        SendFriendAddedEventtoServer(recID, PlayerData.UID, PlayerData.Name);
    }

    [Command]
    private void SendFriendAddedEventtoServer(int recID, int otherID, string otherName)
    {
        Debug.Log("SERVER RECEIVED COMMAND: Sending Friend Addd Event for: " + recID + "from " + otherID);
        FriendAdded(recID, otherID, otherName);
    }

    [ClientRpc]
    private void FriendAdded(int recID, int otherID, string otherName)
    {
        Debug.Log("CLIENT RPC: Received Friend Added Event for: " + recID);

        if (recID == PlayerData.UID)
        {
            BasicInfo basicInfo = new BasicInfo
            {
                UID = otherID,
                Name = otherName
            };
            PlayerData.FriendList.Add(basicInfo);
            FriendsManager.Instance.InvokeOnFriendListUpdated();
        }
    }

    public void SendFriendRemovedEvent(int recID)
    {
        Debug.Log("COMMAND: Sending Friend Removed Event for: " + recID + "from " + PlayerData.UID);
        SendFriendRemovedEventtoServer(recID, PlayerData.UID);
    }

    [Command]
    private void SendFriendRemovedEventtoServer(int recID, int otherID)
    {
        Debug.Log("SERVER RECEIVED COMMAND: Sending Friend Removed Event for: " + recID + "from " + otherID);
        FriendRemoved(recID, otherID);
    }

    [ClientRpc]
    private void FriendRemoved(int recID, int otherID)
    {

        Debug.Log("Received Friend Removed Event for " + recID);
        if (recID == PlayerData.UID)
        {
            PlayerData.FriendList.Remove(PlayerData.FindPlayerInfoByID(otherID));

            FriendInfo info = PlayerData.FindFriendInfoByID(otherID);
            if (info != null)
                PlayerData.FriendDataList.Remove(info);

            FriendsManager.Instance.InvokeOnFriendListUpdated();

        }
    }

    public void SwitchSubScene(string sceneName)
    {
        PlayerInteract.ClearInteractables();
        RequestSwitchToSubScene(PlayerData.activeSubScene, sceneName);
        PlayerData.activeSubScene = sceneName;
    }

    public void EnterSubScene(string sceneName)
    {
        Debug.Log("Send request to enter");
        RequestEnterSubScene(sceneName);
        PlayerData.activeSubScene = sceneName;
    }


    [Command]
    private void RequestSwitchToSubScene(string currSceneName, string sceneName)
    {
        SceneManager.Instance.EnterNetworkedSubScene(netIdentity, currSceneName, sceneName);
    }

    [Command]
    private void RequestEnterSubScene(string sceneName)
    {
        SceneManager.Instance.EnterNetworkedScene(netIdentity, sceneName);
    }

}
