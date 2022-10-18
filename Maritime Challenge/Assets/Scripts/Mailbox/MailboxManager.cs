using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MailboxManager : MonoBehaviourSingleton<MailboxManager>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshMail()
    {
        StartCoroutine(DoRefreshMail());
    }

    IEnumerator DoRefreshMail()
    {
        string url = ServerDataManager.URL_getMailData;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                PlayerData.MailList.Clear();
                PlayerData.MailList.AddRange(JSONDeseralizer.DeseralizeMailData(webreq.downloadHandler.text));
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server Error");
                break;
        }
    }

    public void SendFriendshipGiftMail(int recipientUID, int tokenAmount)
    {
        string mailTitle = "Friendship gift sent by " + PlayerData.Name + "!";
        string mailDescription = "You have recieved " + tokenAmount + " tokens from " + PlayerData.Name + "!";

        StartCoroutine(DoSendMail(recipientUID, mailTitle, mailDescription, tokenAmount));
    }

    IEnumerator DoSendMail(int recipientUID, string mailTitle, string mailDesciption, int mailItemAmount)
    {
        string url = ServerDataManager.URL_addMail;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", recipientUID);
        form.AddField("sMailTitle", mailTitle);
        form.AddField("sMailDescription", mailDesciption);
        form.AddField("iMailItemAmount", mailItemAmount);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server Error");
                break;
        }
    }
}
