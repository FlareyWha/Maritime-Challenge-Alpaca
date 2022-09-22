using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class Server : Mirror.Examples.MultipleAdditiveScenes.MultiSceneNetManager
{


    public override void Start()
    {
        StartServer();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        GameHandler.Instance.OnNewPlayerJoined(conn);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //NetworkServer.SpawnObjects();
      
    }
}
