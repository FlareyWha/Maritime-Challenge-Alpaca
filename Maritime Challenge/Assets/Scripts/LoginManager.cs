using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{
    private string url;
    [SerializeField]
    private Text confirmationText;
    [SerializeField]
    private InputField InputField_Email, InputField_Password;

    NetworkManager manager;

    void Start()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void Login()
    {
        ConnectToServer();
        return;


        // Check Verification of Input Fields from Database blah blah...
        //Verify that email is a proper email. If it is then continue, else try again
        if (!InputField_Email.text.Contains(".com") || !InputField_Email.text.Contains("@"))
        {
            confirmationText.text = "Email is not a proper email. Please try again";
            return;
        }

        // LOGIN
        // Set Player Details/Data in PLayerData
        StartCoroutine(DoSendLoginInfoEmail());
    }

    IEnumerator DoSendLoginInfoEmail()
    {
        //Set the URL to the getUID one
        url = ServerDataManager.Instance.URL_login;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("sEmail", InputField_Email.text);
        form.AddField("sPassword", InputField_Password.text);
        UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("Sending info Success");
                if (webreq.downloadHandler.text == "Login failed.")
                    confirmationText.text = webreq.downloadHandler.text;
                else
                {
                    confirmationText.text = "Login Success";
                    //Save the UID 
                    PlayerData.UID = int.Parse(webreq.downloadHandler.text);
                    Debug.Log(PlayerData.UID);

                    //Connect only if login is successful
                    ConnectToServer();
                }
                break;
            default:
                confirmationText.text = "Server error";
                break;
        }
    }

    private void ConnectToServer()
    {
        // Connect to Server
        SceneManager.Instance.LoadScene(SCENE_ID.HOME);
        foreach (Transform pos in NetworkManager.startPositions)
        {
            Debug.Log("Found Pos: " + pos.position);
        }
        manager.StartClient();
    }
}
