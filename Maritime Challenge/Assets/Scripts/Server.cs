using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class Server : MonoBehaviour
{
    NetworkManager manager;

    void Start()
    {
        manager = GetComponent<NetworkManager>();
        manager.StartServer();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

}
