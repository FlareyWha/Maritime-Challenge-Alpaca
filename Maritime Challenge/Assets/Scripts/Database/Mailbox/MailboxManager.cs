using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MailboxManager : MonoBehaviourSingleton<MailboxManager>
{

    public void DeleteMail(Mail mail)
    {
        StartCoroutine(DoDeleteMail(mail.MailID));
    }

    IEnumerator DoDeleteMail(int mailID)
    {
        string url = ServerDataManager.URL_deleteMail;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iMailID", mailID);
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

    public void SendFriendshipGiftMail(int recipientUID, int tokenAmount)
    {
        string mailTitle = "You received a gift from " + PlayerData.Name + "!";
        string mailDescription = "DELETE";

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
