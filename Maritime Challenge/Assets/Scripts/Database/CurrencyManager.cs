using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CurrencyManager : MonoBehaviourSingleton<CurrencyManager>
{
    public void UpdateTokenAmount(int deltaPrice)
    {
        PlayerData.NumTokens += deltaPrice;
        PlayerData.InvokeNumTokensUpdated();
        StartCoroutine(DoUpdateTokenAmount());

        //Update the UI of the total tokens if needed
    }

    public void UpdateRightShipRollarsAmount(int deltaPrice)
    {
        PlayerData.NumRightshipRollars += deltaPrice;
        PlayerData.InvokeNumRollarsUpdated();
        StartCoroutine(DoUpdateRightshipRollarsAmount());

        //Update the UI of the total tokens if needed
    }

    public static IEnumerator DoUpdateRightshipRollarsAmount()
    {
        string url = ServerDataManager.URL_updateTotalRightshipRollars;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("iTotalRightshipRollars", PlayerData.NumRightshipRollars);
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
                Debug.LogError("Server error");
                break;
        }
    }

    public static IEnumerator DoUpdateTokenAmount()
    {
        string url = ServerDataManager.URL_updateTotalTokens;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("iTotalTokens", PlayerData.NumTokens);
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
                Debug.LogError("Server error");
                break;
        }
    }

    public static IEnumerator DoUpdateEventCurrency()
    {
        string url = ServerDataManager.URL_updateTotalEventCurrency;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("iTotalEventCurrency", PlayerData.NumEventCurrency);
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
                Debug.LogError("Server error");
                break;
        }
    }
}
