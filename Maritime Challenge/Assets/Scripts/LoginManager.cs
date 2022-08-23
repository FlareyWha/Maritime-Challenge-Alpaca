using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LoginManager : MonoBehaviour
{
    NetworkManager manager;

    void Start()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void Login()
    {
        // Check Verification Database blah blah...



        // LOGIN
        // Set Player Details/Data

        // Connect to Server
        SceneManager.Instance.LoadScene(SCENE_ID.HOME);
        foreach (Transform pos in NetworkManager.startPositions)
        {
            Debug.Log("Found Pos: " + pos.position);
        }
        manager.StartClient();
    }
}
