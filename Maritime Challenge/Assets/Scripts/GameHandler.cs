using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameHandler : NetworkBehaviour
{
    [SerializeField]
    private GameObject WarningPanel;

    #region Singleton
    public static GameHandler Instance = null;
    #endregion
    private readonly SyncDictionary<int, Player> onlinePlayers = new SyncDictionary<int, Player>();

    private void Awake()
    {
        Instance = this;
    }

    [Server]
    public void OnNewPlayerJoined(NetworkConnectionToClient conn)
    {
        StartCoroutine(SetPlayerOnline(conn));
    }
    IEnumerator SetPlayerOnline(NetworkConnectionToClient conn)
    {
        while (conn.identity == null)
            yield return null;

        Player player = conn.identity.gameObject.GetComponent<Player>();


        while (player.GetUID() == 0)
            yield return null;

        if (player.GetUID() == -1)
        {
            Debug.Log("Registering Online Player to List..");
            onlinePlayers.Add(conn.connectionId, player);
            yield break;
        }

        foreach (KeyValuePair<int, Player> onlinePlayer in onlinePlayers)
        {
            if (player.GetUID() == onlinePlayer.Value.GetUID())
            {
                ForceKick(player);
                yield break;
            }
        }

        Debug.Log("Registering Online Player to List..");
        onlinePlayers.Add(conn.connectionId, player);
    }

    [ClientRpc]
    private void ForceKick(Player player)
    {
        if (player.isLocalPlayer)
        {
            Instantiate(WarningPanel);
            ConnectionManager.Instance.DisconnectFromServer();
        }
    }

    [Server]
    public void OnPlayerQuit(NetworkConnectionToClient conn)
    {
        Debug.Log("Removing Player from Online List..");
        onlinePlayers.Remove(conn.connectionId);
    }

    public void SendChatMessage(string message)
    {
        Debug.Log("Sending A Message..");
        SendMessageToServer(ChatManager.Instance.GetChatType(), PlayerData.MyPlayer, message);
    }

    [Command(requiresAuthority = false)]
    private void SendMessageToServer(CHAT_TYPE type, Player player, string message)
    {
        Debug.Log("Sending Message to Server");
        UpdateChatLog(type, player, player.GetGuildID(), player.GetUsername(), message);
    }

    [ClientRpc]
    private void UpdateChatLog(CHAT_TYPE chat_type, Player player, int guildID, string playerName, string message)
    {
        Debug.Log("Received RPC from Server, Updating Chat Log...");
        if (chat_type == CHAT_TYPE.GUILD && guildID != PlayerData.GuildID)
            return;

        ChatManager.Instance.UpdateChatLog(chat_type, player.GetUID(), playerName, message);

        if (player == null)
            return;


        PlayerUI playerUI = player.gameObject.GetComponent<PlayerUI>();
        playerUI.AddChatBubble(message);
    }

    public void SendDeletedFriendRequestEvent(int sender_id, int rec_id)
    {
        SendDeletedFriendRequestEventToServer(sender_id, rec_id);
    }

    [Command(requiresAuthority = false)]
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

    [Command(requiresAuthority = false)]
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
            if (!PlayerData.PhonebookData[senderID].Unlocked)
            {
                StartCoroutine(PlayerUI.UpdatePhonebookOtherUnlocked(senderID));
                PlayerData.MyPlayer.UpdateXPLevels(300);
                PlayerStatsManager.Instance.UpdatePlayerStat(PLAYER_STAT.RIGHTSHIPEDIA_ENTRIES_UNLOCKED, ++PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.RIGHTSHIPEDIA_ENTRIES_UNLOCKED]);
            }

            PlayerData.ReceivedFriendRequestList.Add(senderID);
            FriendRequestHandler.InvokeFriendRequestSentEvent(senderID, recID);
        }

    }

    public void SendFriendAddedEvent(int recID)
    {
        Debug.Log("COMMAND: Sending Friend Added Event for: " + recID + "from " + PlayerData.UID);
        SendFriendAddedEventtoServer(recID, PlayerData.UID, PlayerData.Name);
    }

    [Command(requiresAuthority = false)]
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
            PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.FRIENDS_ADDED]++;
            PlayerData.InvokePlayerStatsUpdated();
            FriendsManager.Instance.InvokeOnFriendListUpdated();
        }
    }

    public void SendMailBoxEvent(int recID)
    {
        Debug.Log("COMMAND: Sending Mail Event for: " + recID + "from " + PlayerData.UID);
        SendMailBoxEventtoServer(recID);
    }

    [Command(requiresAuthority = false)]
    private void SendMailBoxEventtoServer(int recID)
    {
        Debug.Log("SERVER RECEIVED COMMAND: Sending Mail Event for: " + recID);
        MailBoxReceived(recID);
    }

    [ClientRpc]
    private void MailBoxReceived(int recID)
    {
        Debug.Log("CLIENT RPC: Received Mail Event for: " + recID);
        if (PlayerData.UID == recID)
            MailboxUIManager.Instance.UpdateMailRect();
    }

    public void SendFriendRemovedEvent(int recID)
    {
        Debug.Log("COMMAND: Sending Friend Removed Event for: " + recID + "from " + PlayerData.UID);
        SendFriendRemovedEventtoServer(recID, PlayerData.UID);
    }

    [Command(requiresAuthority = false)]
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
            PlayerData.FriendList.Remove(PlayerData.FindPlayerFromFriendList(otherID));

            FriendInfo info = PlayerData.FindFriendInfoByID(otherID);
            if (info != null)
                PlayerData.FriendDataList.Remove(info);

            FriendsManager.Instance.InvokeOnFriendListUpdated();

        }
    }

}
