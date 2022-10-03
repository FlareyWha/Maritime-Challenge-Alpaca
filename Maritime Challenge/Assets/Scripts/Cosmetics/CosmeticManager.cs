using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CosmeticManager : MonoBehaviour
{
    private List<Cosmetic> cosmeticList = new List<Cosmetic>();
    private Dictionary<int, bool> cosmeticStatusDictionary = new Dictionary<int, bool>();

    public void GetCosmetics()
    {
        StartCoroutine(DoGetCosmetics());
        StartCoroutine(DoGetCosmeticStatusList());
    }

    IEnumerator DoGetCosmetics()
    {
        string url = ServerDataManager.URL_getCosmeticData;
        Debug.Log(url);

        using UnityWebRequest webreq = UnityWebRequest.Get(url);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                cosmeticList.AddRange(JSONDeseralizer.DeseralizeCosmeticData(webreq.downloadHandler.text));
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator DoGetCosmeticStatusList()
    {
        string url = ServerDataManager.URL_getCosmeticStatusList;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //I sure hope this wont break things in the future
                cosmeticStatusDictionary = JSONDeseralizer.DeseralizeCosmeticStatusList(webreq.downloadHandler.text);
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


