using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TitleManager : MonoBehaviour
{
    private Dictionary<Title, bool> titleDictionary = new Dictionary<Title, bool>();

    public void GetTitles()
    {
        StartCoroutine(DoGetTitles());
        StartCoroutine(DoGetTitleStatusList());
    }

    IEnumerator DoGetTitles()
    {
        string url = ServerDataManager.URL_getTitleData;
        Debug.Log(url);

        using UnityWebRequest webreq = UnityWebRequest.Get(url);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                titleDictionary = JSONDeseralizer.DeseralizeTitleData(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator DoGetTitleStatusList()
    {
        string url = ServerDataManager.URL_getTitleStatusList;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //I sure hope this wont break things in the future
                titleStatusDictionary = JSONDeseralizer.DeseralizeTitleStatusList(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }
}
