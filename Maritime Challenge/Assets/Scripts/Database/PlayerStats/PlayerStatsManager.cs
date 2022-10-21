using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStatsManager : MonoBehaviourSingleton<PlayerStatsManager>
{
    protected override void Awake()
    {
        base.Awake();

        UpdatePlayerStat(PLAYER_STAT.LOGIN, ++PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.LOGIN]);
    }
    public void UpdatePlayerStat(PLAYER_STAT playerStat, int statAmount)
    {
        StartCoroutine(DoUpdatePlayerStat(PlayerData.PlayerStats.statNames[(int)playerStat], statAmount));
    }

    static IEnumerator DoUpdatePlayerStat(string statName, int statAmount)
    {
        string url = ServerDataManager.URL_updatePlayerStats;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        form.AddField("iStatName", statName);
        form.AddField("iStatAmount", statAmount);
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
                Debug.LogError(webreq.downloadHandler.text);
                break;
        }
    }
}
