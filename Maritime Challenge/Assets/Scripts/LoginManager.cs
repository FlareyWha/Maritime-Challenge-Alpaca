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

    [SerializeField]
    private GameObject LoadingScreen;

    private PostLoginInfoGetter postLoginInfoGetter;


    private void Start()
    {
        postLoginInfoGetter = GetComponent<PostLoginInfoGetter>();
    }

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
        PlayerData.ResetData();
        LoadingScreen.gameObject.SetActive(true);

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
                StartCoroutine(UpdateLastLoginTime());
                break;
            case UnityWebRequest.Result.ProtocolError:
                confirmationText.text = webreq.downloadHandler.text;
                LoadingScreen.gameObject.SetActive(false);
                break;
            default:
                Debug.Log(webreq.downloadHandler.text);
                confirmationText.text = "Server error";
                LoadingScreen.gameObject.SetActive(false);
                break;
        }
    }

    IEnumerator UpdateLastLoginTime()
    {
        //Set the URL to the getUID one
        url = ServerDataManager.URL_updateLastLoginTime;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);
                //Get player data
                StartCoroutine(postLoginInfoGetter.GetInfo());
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                LoadingScreen.gameObject.SetActive(false);
                break;
            default:
                Debug.LogError(webreq.downloadHandler.text);
                LoadingScreen.gameObject.SetActive(false);
                break;
        }
    }

    public void ConnectToServer()
    {
        // Connect to Server
        ConnectionManager.Instance.ConnectToServer();
    }

    public void GuestLogin()
    {
        LoadingScreen.gameObject.SetActive(true);
        PlayerData.InitGuestData();
        ConnectToServer();
        LoadTutorial();
    }
    private void LoadTutorial()
    {
        StartCoroutine(TutorialLoading());
    }

    IEnumerator TutorialLoading()
    {
        while (PlayerData.CommandsHandler == null)
            yield return null;

        PlayerData.CommandsHandler.SwitchSubScene("TutorialScene", SceneManager.TutorialSpawnPos);
    }
}
