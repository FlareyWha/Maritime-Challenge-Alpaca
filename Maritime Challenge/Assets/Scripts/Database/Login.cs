using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

public class Login : MonoBehaviour
{
    private string url;
    [SerializeField]
    private Text displayTxt; //must add using UnityEngine.UI
    [SerializeField]
    private InputField if_email, if_password; //to link to the input fields
    [SerializeField]
    private GameObject panel_next;

    private void Awake()
    {
        panel_next.SetActive(false);
    }

    public void SendLoginInfo() //to be invoked by button click
    {
        StartCoroutine(DoSendLoginInfoEmail());
    }

    IEnumerator DoSendLoginInfoEmail()
    {
        //Set the URL to the getUID one
        url = ServerDataManager.Instance.URL_login;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("sEmail", if_email.text);
        form.AddField("sPassword", if_password.text);
        UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("Sending info Success");
                if (webreq.downloadHandler.text == "Login failed.")
                    displayTxt.text = webreq.downloadHandler.text;
                else
                {
                    PlayerData.UID = int.Parse(webreq.downloadHandler.text);
                    Debug.Log(PlayerData.UID);
                    StartCoroutine(DoStartLogin());
                }

                break;
            default:
                displayTxt.text = "Server error";
                break;
        }
    }

    IEnumerator DoStartLogin()
    {
        url = ServerDataManager.Instance.URL_login;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("uid", PlayerData.UID);
        UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("Login success");
                displayTxt.text = webreq.downloadHandler.text;
                
                panel_next.SetActive(true);
                break;
            default:
                displayTxt.text = "Server error";
                break;
        }
    }
}
