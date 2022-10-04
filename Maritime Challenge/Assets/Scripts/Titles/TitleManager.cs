using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TitleManager : MonoBehaviour
{

    //public void GetTitles()
    //{
    //    StartCoroutine(DoGetTitles());
    //    //StartCoroutine(DoGetTitleStatusList());
    //}

   

    //IEnumerator DoGetTitleStatusList()
    //{
    //    string url = ServerDataManager.URL_getTitleStatusList;
    //    Debug.Log(url);

    //    WWWForm form = new WWWForm();
    //    form.AddField("iOwnerUID", PlayerData.UID);
    //    using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
    //    yield return webreq.SendWebRequest();
    //    switch (webreq.result)
    //    {
    //        case UnityWebRequest.Result.Success:
    //            //I sure hope this wont break things in the future
    //            titleStatusDictionary = JSONDeseralizer.DeseralizeTitleStatusList(webreq.downloadHandler.text);
    //            break;
    //        case UnityWebRequest.Result.ProtocolError:
    //            Debug.LogError(webreq.downloadHandler.text);
    //            break;
    //        default:
    //            Debug.LogError("Server error");
    //            break;
    //    }
    //}
}
