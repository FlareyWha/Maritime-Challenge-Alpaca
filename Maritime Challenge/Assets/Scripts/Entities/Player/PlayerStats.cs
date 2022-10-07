using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : MonoBehaviour
{
    public int[] PlayerStat = new int[(int)PLAYER_STAT.NO_PLAYER_STAT];

    private readonly string[] statNames = { "iEnemiesDefeated", "iBossesDefeated", "iFriendsAdded", "iRightshipediaEntriesUnlocked", "iBattleshipsOwned", "iCosmeticsOwned", "iTitlesUnlocked", "iAchievementsCompleted" };

    public void UpdatePlayerStat(PLAYER_STAT playerStat, int statAmount)
    {
        StartCoroutine(DoUpdatePlayerStat(statNames[(int)playerStat], statAmount));
    }

    IEnumerator DoUpdatePlayerStat(string statName, int statAmount)
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

public enum PLAYER_STAT
{
    ENEMIES_DEFEATED,
    BOSSES_DEFEATED,
    FRIENDS_ADDED,
    RIGHTSHIPEDIA_ENTRIES_UNLOCKED,
    BATTLESHIPS_OWNED,
    COSMETICS_OWNED,
    TITLES_UNLOCKED,
    ACHIEVEMENTS_COMPLETED,
    NO_PLAYER_STAT
}
