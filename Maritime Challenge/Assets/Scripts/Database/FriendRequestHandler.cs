using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class FriendRequestHandler
{
    public delegate void FriendRequestSent(int senderID, int recID);
    public static event FriendRequestSent OnFriendRequestSent;

    public delegate void FriendRequestDeleted(int senderID, int recID);
    public static event FriendRequestDeleted OnFriendRequestDeleted;

    public static IEnumerator StartSendFriendRequest(int friendUID)
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
                // PlayerData.SentFriendRequestList.Add(friendUID);
                // OnFriendRequestSent?.Invoke(friendUID);
                GameHandler.Instance.SendFriendRequestEvent(PlayerData.UID, friendUID);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    public static IEnumerator StartDeleteFriendRequest(int senderID, int recID)
    {
        string url = ServerDataManager.URL_deleteFriendRequest;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", senderID);
        form.AddField("iOtherUID", recID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                Debug.Log(webreq.downloadHandler.text);
                GameHandler.Instance.SendDeletedFriendRequestEvent(senderID, recID);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    public static IEnumerator GetSentFriendRequests()
    {
        string url = ServerDataManager.URL_getSentFriendRequests;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
               
                //Deseralize the data
                JSONDeseralizer.DeseralizeSentFriendRequests(webreq.downloadHandler.text);
                Debug.Log("Get Sent Friend Requests Success");
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    public static IEnumerator GetRecievedFriendRequests()
    {
        string url = ServerDataManager.URL_getReceivedFriendRequests;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                JSONDeseralizer.DeseralizeReceivedFriendRequests(webreq.downloadHandler.text);
                Debug.Log("Get Received Friend Requests Success");

                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    public static void InvokeFriendRequestSentEvent(int sender_id, int rec_id)
    {
        OnFriendRequestSent?.Invoke(sender_id, rec_id);
    }

    public static void InvokeFriendRequestDeletedEvent(int sender_id, int rec_id)
    {
        OnFriendRequestDeleted?.Invoke(sender_id, rec_id);
    }
}
