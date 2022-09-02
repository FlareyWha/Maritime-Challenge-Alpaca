using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{
    private string url;
    [SerializeField]
    private Text confirmationText;
    [SerializeField]
    private InputField InputField_Email, InputField_Password;


    public void Login()
    {
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
        url = ServerDataManager.URL_login;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("sEmail", InputField_Email.text);
        form.AddField("sPassword", InputField_Password.text);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);
                Debug.Log("Sending info Success");

                confirmationText.text = "Login Success";

                //Save the UID 
                PlayerData.UID = int.Parse(webreq.downloadHandler.text);
                Debug.Log(PlayerData.UID);

                //Get player data
                StartCoroutine(GetPlayerData());
                break;
            case UnityWebRequest.Result.ProtocolError:
                confirmationText.text = webreq.downloadHandler.text;
                break;
            default:
                Debug.Log(webreq.downloadHandler.text);
                confirmationText.text = "Server error";
                break;
        }
    }

    IEnumerator GetPlayerData()
    {
        url = ServerDataManager.URL_getPlayerData;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                JSONDeseralizer.DeseralizePlayerData(webreq.downloadHandler.text);

                //Get friends
                StartCoroutine(GetFriends());
                break;
            case UnityWebRequest.Result.ProtocolError:
                confirmationText.text = webreq.downloadHandler.text;
                break;
            default:
                confirmationText.text = "Server error";
                break;
        }
    }

    IEnumerator GetFriends()
    {
        url = ServerDataManager.URL_getFriends;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                JSONDeseralizer.DeseralizeFriends(webreq.downloadHandler.text);

                //Get phonebook data
                StartCoroutine(GetPhonebookData());
                break;
            case UnityWebRequest.Result.ProtocolError:
                confirmationText.text = webreq.downloadHandler.text;
                break;
            default:
                confirmationText.text = "Server error";
                break;
        }
    }

    IEnumerator GetPhonebookData()
    {
        url = ServerDataManager.URL_getPhonebookData;
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

                //Set player to online
                StartCoroutine(SetOnline());
                break;
            case UnityWebRequest.Result.ProtocolError:
                confirmationText.text = webreq.downloadHandler.text;
                break;
            default:
                confirmationText.text = "Server error";
                break;
        }
    }

    IEnumerator SetOnline()
    {
        url = ServerDataManager.URL_setOnline;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("bOnline", 1);
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);

                ServerManager.Instance.OfflineSetted = false;

                //Connect only if login is successful
                ConnectToServer();
                break;
            case UnityWebRequest.Result.ProtocolError:
                confirmationText.text = webreq.downloadHandler.text;
                break;
            default:
                confirmationText.text = "Server error";
                break;
        }
    }

    public void ConnectToServer()
    {
        // Connect to Server
        SceneManager.Instance.LoadScene(SCENE_ID.HOME);
        ServerManager.Instance.ConnectToServer();
      
    }
}
