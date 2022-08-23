using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

public class Login : MonoBehaviour
{
    public string URL = "http://localhost/lab02_daevon_200412T/login.php";
    public Text displayTxt; //must add using UnityEngine.UI
    public InputField if_email, if_password; //to link to the input fields
    public GameObject panel_next;

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
        URL = DataManager.Instance.URL_getUid;
        Debug.Log(URL);

        WWWForm form = new WWWForm();
        form.AddField("sEmail", if_email.text);
        form.AddField("sPassword", if_password.text);
        UnityWebRequest webreq = UnityWebRequest.Post(URL, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("Sending info Success");
                if (webreq.downloadHandler.text == "Login failed.")
                    displayTxt.text = webreq.downloadHandler.text;
                else
                {
                    DataManager.Instance.UID = int.Parse(webreq.downloadHandler.text);
                    Debug.Log(DataManager.Instance.UID);
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
        URL = DataManager.Instance.URL_login;
        Debug.Log(URL);

        WWWForm form = new WWWForm();
        form.AddField("uid", DataManager.Instance.UID);
        UnityWebRequest webreq = UnityWebRequest.Post(URL, form);
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
