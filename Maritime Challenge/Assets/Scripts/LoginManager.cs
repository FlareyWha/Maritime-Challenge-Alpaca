using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LoginManager : MonoBehaviour
{
    [SerializeField]
    private InputField InputField_Username, InputField_Password;

    NetworkManager manager;

    void Start()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void Login()
    {
        // Check Verification of Input Fields from Database blah blah...



        // LOGIN
        // Set Player Details/Data in PLayerData



        // Connect to Server
        SceneManager.Instance.LoadScene(SCENE_ID.HOME);
        foreach (Transform pos in NetworkManager.startPositions)
        {
            Debug.Log("Found Pos: " + pos.position);
        }
        manager.StartClient();
    }
}
