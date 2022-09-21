using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Networking;

public class ServerManager : NetworkManager
{
    #region Singleton
    public static ServerManager Instance = null;
    #endregion

    private bool offlineSetted = false;
    public bool OfflineSetted
    {
        get { return offlineSetted; }
        set { offlineSetted = value; }
    }

   // private NetworkManager manager;

    public override void Start()
    {
        Instance = this;
       // manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }


    public void ConnectToServer()
    {
        StartClient();
    }


    public void DisconnectFromServer()
    {
        StopClient();
    }

    private void LogOutOfGame()
    {
        SceneManager.Instance.LoadScene(SCENE_ID.LOGIN);
        PlayerData.ResetData();
    }
}
