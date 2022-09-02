using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviourSingleton<ServerManager>
{
    private bool offlineSetted = false;
    public bool OfflineSetted
    {
        get { return offlineSetted; }
        set { offlineSetted = value; }
    }

    private NetworkManager manager;

    void Start()
    {
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    public void ConnectToServer()
    {
        manager.StartClient();
    }


    public void DisconnectFromServer()
    {
        TrySetOffline();

        Debug.Log("Disconencting Client from Server...");
        NetworkManager.singleton.StopClient();
    }

    private void OnApplicationQuit()
    {
        TrySetOffline();
    }

    void TrySetOffline()
    {
        if (!offlineSetted)
        {
            StartCoroutine(SetOffline());
            offlineSetted = true;
        }
    }

    IEnumerator SetOffline()
    {
        string url = ServerDataManager.URL_setOnline;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("bOnline", 0);
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
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
}
