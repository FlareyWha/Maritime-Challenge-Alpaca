using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FriendsManager : MonoBehaviourSingleton<FriendsManager>
{

    public delegate void FriendListUpdated();
    public static event FriendListUpdated OnFriendListUpdated;

    public void SendFriendRequest(int friendUID)
    {
        //Add UI

        //Start friend request
        StartCoroutine(StartSendFriendRequest(friendUID));
    }

    IEnumerator StartSendFriendRequest(int friendUID)
    {
        string url = ServerDataManager.URL_addFriendRequest;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("iOtherUID", friendUID);
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

    public static bool CheckIfFriends(int id)
    {
        Debug.Log("Checking through Friends List...");
        foreach (BasicInfo player in PlayerData.FriendList)
        {
            Debug.Log("Found Friend Name: " + player.Name);
            if (player.UID == id)
                return true;
        }
        return false;
    }
}
