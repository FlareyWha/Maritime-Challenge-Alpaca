using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PostLoginInfoGetter : MonoBehaviour
{
    private LoginManager loginManager;

    private void Start()
    {
        loginManager = GetComponent<LoginManager>();
    }

    public IEnumerator GetInfo()
    {
        CoroutineCollection coroutineCollectionManager = new CoroutineCollection();

        StartCoroutine(coroutineCollectionManager.CollectCoroutine(GetPlayerData()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(GetFriends()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(GetPhonebookData()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(GetSentFriendRequests()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(GetRecievedFriendRequests()));

        //Wait for all the coroutines to finish running before continuing
        yield return coroutineCollectionManager;

        //Set online once all the info has been recieved
        loginManager.SetOnline();
    }

    IEnumerator GetPlayerData()
    {
        string url = ServerDataManager.URL_getPlayerData;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("Get player data success");

                //Deseralize the data
                JSONDeseralizer.DeseralizePlayerData(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator GetFriends()
    {
        string url = ServerDataManager.URL_getFriends;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("Get friends success");

                //Deseralize the data
                JSONDeseralizer.DeseralizeFriends(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator GetPhonebookData()
    {
        string url = ServerDataManager.URL_getPhonebookData;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                JSONDeseralizer.DeseralizePhonebookData(webreq.downloadHandler.text);

                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator GetSentFriendRequests()
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

                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator GetRecievedFriendRequests()
    {
        string url = ServerDataManager.URL_getRecievedFriendRequests;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                JSONDeseralizer.DeseralizeRecievedFriendRequests(webreq.downloadHandler.text);

                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }
}
