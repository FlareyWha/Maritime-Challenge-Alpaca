using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FriendsManager : MonoBehaviourSingleton<FriendsManager>
{
    [SerializeField]
    private GameObject IncomingFriendRequestUIPrefab, PendingFriendRequestUIPrefab;
    [SerializeField]
    private Transform IncomingListRect, PendingListRect;


    public delegate void FriendListUpdated();
    public static event FriendListUpdated OnFriendListUpdated;

    public delegate void NewFriendDataSaved(FriendInfo info);
    public static event NewFriendDataSaved OnNewFriendDataSaved;

    private void Start()
    {
        UpdateRequestsPanelUI();
        FriendRequestHandler.OnFriendRequestSent += OnFriendRequestsUpdated;
        FriendRequestHandler.OnFriendRequestDeleted += OnFriendRequestsUpdated;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        FriendRequestHandler.OnFriendRequestSent -= OnFriendRequestsUpdated;
        FriendRequestHandler.OnFriendRequestDeleted -= OnFriendRequestsUpdated;
    }

    private void UpdateRequestsPanelUI()
    {
        // Clear Rects
        foreach (Transform child in IncomingListRect)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in PendingListRect)
        {
            Destroy(child.gameObject);
        }

        // Fill in Incoming Rect
        foreach (int id in PlayerData.ReceivedFriendRequestList)
        {
            GameObject uiGO = Instantiate(IncomingFriendRequestUIPrefab, IncomingListRect);
            IncomingFriendRequestUI ui = uiGO.GetComponent<IncomingFriendRequestUI>();
            ui.Init(id);
        }

        // Fill in Pending Rect
        foreach (int id in PlayerData.SentFriendRequestList)
        {
            GameObject uiGO = Instantiate(PendingFriendRequestUIPrefab, PendingListRect);
            PendingFriendRequestUI ui = uiGO.GetComponent<PendingFriendRequestUI>();
            ui.Init(id);
        }
    }

    private void OnFriendRequestsUpdated(int sender_id, int receiver_id)
    {
        UpdateRequestsPanelUI();
    }


    public void SendFriendRequest(int friendUID)
    {
        //Add UI

        //Start friend request
        StartCoroutine(FriendRequestHandler.StartSendFriendRequest(friendUID));
    }


    public void DeleteFriendRequest(int senderID, int recID)
    {
        //Add UI

        //Start friend request
        StartCoroutine(FriendRequestHandler.StartDeleteFriendRequest(senderID, recID));
    }
    public void AddFriend(int id, string name)
    {
        StartCoroutine(StartAddFriend(id, name));
    }
    IEnumerator StartAddFriend(int otherUID, string name)
    {
        string url = ServerDataManager.URL_addFriend;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("OtherUID", otherUID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);

                //Add friend to the friend list
                BasicInfo basicInfo = new BasicInfo
                {
                    UID = otherUID,
                    Name = name
                };
                PlayerData.FriendList.Add(basicInfo);
                PlayerStatsManager.Instance.UpdatePlayerStat(PLAYER_STAT.FRIENDS_ADDED, ++PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.FRIENDS_ADDED]);
                GameHandler.Instance.SendFriendAddedEvent(otherUID);
                OnFriendListUpdated?.Invoke();
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Friend cannot be added");
                break;
        }
    }
    public void DeleteFriend(int id)
    {
        //Delete twice due to how friends work
        StartCoroutine(StartDeleteFriend(id));
    }
    IEnumerator StartDeleteFriend(int otherUID)
    {
        string url = ServerDataManager.URL_deleteFriend;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("OtherUID", otherUID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);
                PlayerData.FriendList.Remove(PlayerData.FindPlayerFromFriendList(otherUID));
                PlayerData.FriendDataList.Remove(PlayerData.FindFriendInfoByID(otherUID));
                GameHandler.Instance.SendFriendRemovedEvent(otherUID);
                OnFriendListUpdated?.Invoke();
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Friend cannot be removed");
                break;
        }
    }

    public void GetFriendDataInfo(int friendUID)
    {
        StartCoroutine(StartGetFriendInfo(friendUID));
    }
    IEnumerator StartGetFriendInfo(int friendUID)
    {
        string url = ServerDataManager.URL_getFriendInfo;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("iFriendUID", friendUID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                FriendInfo friend = JSONDeseralizer.DeseralizeFriendData(friendUID, webreq.downloadHandler.text);
                OnNewFriendDataSaved?.Invoke(friend);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    public void UpdateFriendshipXPLevels(int friendUID, int xpGained)
    {
        //Need to find way to update this on both clients.
        //Either this function gets called twice or this will invoke smth that doesnt do the calculations but just update the numbers
        //Contact me if need discussing I suppose
        //For now what this func does is update on one client and the database.

        FriendInfo friend = null;

        foreach (FriendInfo friendInfo in PlayerData.FriendDataList)
        {
            if (friendInfo.UID == friendUID)
            {
                friend = friendInfo;
                break;
            }
        }

        if (friend != null)
        {
            int currXP = friend.FriendshipXP + xpGained;
            int currLevel = friend.FriendshipLevel;
            int friendshipLevel = 0, friendshipXP = 0;
            bool finishedLevelingUp = false;

            do
            {
                //Get the xp requirement
                int xpRequirement = GameSettings.GetFriendshipXPRequirement(currLevel);

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

            friend.FriendshipLevel = currLevel;
            friend.FriendshipXP = currXP;

            StartCoroutine(DoUpdateFriendshipXPLevels(PlayerData.UID, friendUID, friendshipLevel, friendshipXP));
            StartCoroutine(DoUpdateFriendshipXPLevels(friendUID, PlayerData.UID, friendshipLevel, friendshipXP));
        }
    }

    IEnumerator DoUpdateFriendshipXPLevels(int ownerUID, int friendUID, int friendshipLevel, int friendshipXP)
    {
        string url = ServerDataManager.URL_updateFriendshipXPLevels;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", ownerUID);
        form.AddField("iFriendUID", friendUID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                FriendInfo friend = JSONDeseralizer.DeseralizeFriendData(friendUID, webreq.downloadHandler.text);
                OnNewFriendDataSaved?.Invoke(friend);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    public void InvokeOnFriendListUpdated()
    {
        OnFriendListUpdated?.Invoke();
    }
    public static bool CheckIfFriends(int id)
    {
        foreach (BasicInfo player in PlayerData.FriendList)
        {
            if (player.UID == id)
                return true;
        }
        return false;
    }

    public static bool CheckIfPending(int id)
    {
        foreach (int sent_id in PlayerData.SentFriendRequestList)
        {
            if (sent_id == id)
                return true;
        }
        return false;
    }

    public static bool CheckIfIncoming(int id)
    {
        foreach (int sent_id in PlayerData.ReceivedFriendRequestList)
        {
            if (sent_id == id)
                return true;
        }
        return false;
    }
}
