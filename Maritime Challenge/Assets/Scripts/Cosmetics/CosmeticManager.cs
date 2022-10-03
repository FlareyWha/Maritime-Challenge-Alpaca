using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CosmeticManager : MonoBehaviour
{
    private List<Cosmetic> cosmeticList = new List<Cosmetic>();

    public void GetCosmetics()
    {
        StartCoroutine(DoStartCosmetics());
    }

    IEnumerator DoStartCosmetics()
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
}


