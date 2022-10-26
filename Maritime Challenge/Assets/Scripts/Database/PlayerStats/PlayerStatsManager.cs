using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStatsManager : MonoBehaviourSingleton<PlayerStatsManager>
{
    public void UpdatePlayerStat(PLAYER_STAT playerStat, int statAmount)
    {
        PlayerData.PlayerStats.PlayerStat[(int)playerStat] = statAmount;
        PlayerData.InvokePlayerStatsUpdated();
        StartCoroutine(DoUpdatePlayerStat(PlayerData.PlayerStats.statNames[(int)playerStat], statAmount));
    }

    static IEnumerator DoUpdatePlayerStat(string statName, int statAmount)
    {
        string url = ServerDataManager.URL_updatePlayerStats;
        Debug.Log(url);


        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        form.AddField("sStatName", statName);
        form.AddField("iStatAmount", statAmount);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("Player Stats Updated: " + statName + " set to " + statAmount);
                Debug.Log(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError(webreq.downloadHandler.text);
                break;
        }
    }
}
