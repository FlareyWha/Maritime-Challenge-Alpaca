using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerManager : MonoBehaviourSingleton<ServerManager>
{
 
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
        Debug.Log("Disconencting Client from Server...");
        NetworkManager.singleton.StopClient();
        Debug.Log("Switching back to Login Scene...");
        SceneManager.Instance.LoadScene(SCENE_ID.LOGIN);
        Debug.Log("Resetting Player Data...");
        PlayerData.ResetData();
    }
}
