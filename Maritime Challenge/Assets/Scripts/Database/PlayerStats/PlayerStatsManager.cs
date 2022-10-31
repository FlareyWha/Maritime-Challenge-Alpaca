using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStatsManager : MonoBehaviourSingleton<PlayerStatsManager>
{
    public void UpdatePlayerStat(PLAYER_STAT playerStat, int statAmount)
    {
        Debug.Log("Player " + playerStat + " set to " + statAmount);
        PlayerData.PlayerStats.PlayerStat[(int)playerStat] = statAmount;
        PlayerData.InvokePlayerStatsUpdated();
        StartCoroutine(DoUpdatePlayerStat(PlayerData.PlayerStats.statNames[(int)playerStat], statAmount));
    }

    public void SaveAllStats()
    {
        UpdateStats();
        StartCoroutine(DoSaveAllStatsOnQuit());
    }

    void UpdateStats()
    {
        PlayerStats playerStats = PlayerData.PlayerStats;

        playerStats.PlayerStat[(int)PLAYER_STAT.FRIENDS_ADDED] = GetNumberOfFriends();
        playerStats.PlayerStat[(int)PLAYER_STAT.RIGHTSHIPEDIA_ENTRIES_UNLOCKED] = GetRightshipediaUnlockedEntriesNumber();
        playerStats.PlayerStat[(int)PLAYER_STAT.BATTLESHIPS_OWNED] = GetOwnedBattleships();
        playerStats.PlayerStat[(int)PLAYER_STAT.COSMETICS_OWNED] = GetOwnedCosmetics();
        playerStats.PlayerStat[(int)PLAYER_STAT.TITLES_UNLOCKED] = GetUnlockedTitles();
        playerStats.PlayerStat[(int)PLAYER_STAT.ACHIEVEMENTS_COMPLETED] = GetCompletedAchievements();
    }

    int GetNumberOfFriends()
    {
        return PlayerData.FriendDataList.Count;
    }

    int GetRightshipediaUnlockedEntriesNumber()
    {
        int unlockedEntries = 0;

        foreach (KeyValuePair<int, BasicInfo> phonebookEntry in PlayerData.PhonebookData)
        {
            if (phonebookEntry.Value.Unlocked)
                unlockedEntries++;
        }

        return unlockedEntries;
    }

    int GetOwnedBattleships()
    {
        int ownedBattleships = 0;

        foreach (KeyValuePair<BattleshipInfo, bool> battleship in PlayerData.BattleshipList)
        {
            if (battleship.Value)
                ownedBattleships++;
        }

        return ownedBattleships;
    }

    int GetOwnedCosmetics()
    {
        int ownedCosmetics = 0;

        foreach (KeyValuePair<Cosmetic, bool> cosmetic in PlayerData.CosmeticsList)
        {
            if (cosmetic.Value)
                ownedCosmetics++;
        }

        return ownedCosmetics;
    }

    int GetUnlockedTitles()
    {
        int unlockedTitles = 0;

        foreach (KeyValuePair<Title, bool> title in PlayerData.TitleDictionary)
        {
            if (title.Value)
                unlockedTitles++;
        }

        return unlockedTitles;
    }

    int GetCompletedAchievements()
    {
        int completedAchievements = 0;

        foreach (KeyValuePair<Achievement, bool> achievement in PlayerData.AchievementList)
        {
            if (achievement.Value)
                completedAchievements++;
        }

        return completedAchievements;
    }

    IEnumerator DoSaveAllStatsOnQuit()
    {
        CoroutineCollection coroutineCollectionManager = new CoroutineCollection();

        PlayerStats playerStats = PlayerData.PlayerStats;

        for (int i = 0; i < playerStats.statNames.Length; ++i)
        {
            StartCoroutine(coroutineCollectionManager.CollectCoroutine(DoUpdatePlayerStat(playerStats.statNames[i], playerStats.PlayerStat[i])));
        }

        yield return coroutineCollectionManager;
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

    public void UpdateLastPosition(Vector3Int position)
    {
        StartCoroutine(DoUpdateLastPosition(position));
    }

    IEnumerator DoUpdateLastPosition(Vector3Int position)
    {
        string url = ServerDataManager.URL_updateLastPosition;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("fXPlayerPos", position.x);
        form.AddField("fYPlayerPos", position.y);
        form.AddField("fZPlayerPos", position.z);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("Player Position Updated: " + position);
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

    public void UpdateXPLevels(int xpGained)
    {
        int currXP = PlayerData.CurrXP;
        int currLevel = PlayerData.CurrLevel;
        bool finishedLevelingUp = false;

        //Add up all the XP
        currXP += xpGained;

        do
        {
            //Get the xp requirement
            int xpRequirement = GameSettings.GetEXPRequirement(currLevel);

            //Increase level if currXP meets the xpRequirement
            if (currXP >= xpRequirement)
            {
                currLevel++;
                currXP -= xpRequirement;
            }
            else
                finishedLevelingUp = true;
        }
        while (!finishedLevelingUp);

        //Update player data values
        PlayerData.CurrXP = currXP;
        PlayerData.CurrLevel = currLevel;

        //Update database
        StartCoroutine(StartUpdateXPLevels());
    }

    IEnumerator StartUpdateXPLevels()
    {
        string url = ServerDataManager.URL_updateAccountXPLevels;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("iLevel", PlayerData.CurrLevel);
        form.AddField("iXP", PlayerData.CurrXP);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
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
