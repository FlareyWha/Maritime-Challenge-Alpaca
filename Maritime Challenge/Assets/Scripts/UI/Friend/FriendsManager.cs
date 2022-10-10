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
                GameHandler.Instance.SendFriendAddedEvent(otherUID);
                OnFriendListUpdated?.Invoke();
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(webreq.downloadHandler.text);
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
                PlayerData.FriendList.Remove(PlayerData.FindPlayerInfoByID(otherUID));
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
