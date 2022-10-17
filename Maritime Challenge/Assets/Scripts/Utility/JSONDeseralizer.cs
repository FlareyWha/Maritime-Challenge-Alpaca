using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONDeseralizer : MonoBehaviour
{
    public static void DeseralizePlayerData(string playerDataJSON)
    {
        JSONPlayerDataList playerDataList = JsonUtility.FromJson<JSONPlayerDataList>(playerDataJSON);

        JSONPlayerData jsonPlayerData = playerDataList.playerData[0];

        //Set all the info needed
        PlayerData.Name = jsonPlayerData.sUsername;
        PlayerData.ShowBirthday = jsonPlayerData.bShowBirthday;
        PlayerData.Birthday = jsonPlayerData.dBirthday;
        PlayerData.CurrentTitleID = jsonPlayerData.iCurrentTitleID;
        PlayerData.Biography = jsonPlayerData.sBiography;
        PlayerData.CurrLevel = jsonPlayerData.iLevel;
        PlayerData.CurrXP = jsonPlayerData.iXP;
        PlayerData.Department = jsonPlayerData.iDepartment;
        PlayerData.GuildID = jsonPlayerData.iGuildID;
        PlayerData.Country = jsonPlayerData.iCountry;
        PlayerData.NumRightshipRollars = jsonPlayerData.iTotalRightshipRollars;
        PlayerData.NumTokens = jsonPlayerData.iTotalTokens;
        PlayerData.NumEventCurrency = jsonPlayerData.iTotalEventCurrency;
        PlayerData.CurrentBattleship = jsonPlayerData.iCurrentBattleship;
        PlayerData.PlayerPosition = new Vector3(jsonPlayerData.fPlayerXPos, jsonPlayerData.fPlayerYPos, jsonPlayerData.fPlayerZPos);
    }

    public static void DeseralizePlayerStats(string playerStatsJSON)
    {
        JSONPlayerStatsList playerStatsList = JsonUtility.FromJson<JSONPlayerStatsList>(playerStatsJSON);

        JSONPlayerStats jsonPlayerStats = playerStatsList.playerStats[0];

        //Set all the info needed
        PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.ENEMIES_DEFEATED] = jsonPlayerStats.iEnemiesDefeated;
        PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.BOSSES_DEFEATED] = jsonPlayerStats.iBossesDefeated;
        PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.FRIENDS_ADDED] = jsonPlayerStats.iFriendsAdded;
        PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.RIGHTSHIPEDIA_ENTRIES_UNLOCKED] = jsonPlayerStats.iRightshipediaEntriesUnlocked;
        PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.BATTLESHIPS_OWNED] = jsonPlayerStats.iBattleshipsOwned;
        PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.COSMETICS_OWNED] = jsonPlayerStats.iCosmeticsOwned;
        PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.TITLES_UNLOCKED] = jsonPlayerStats.iTitlesUnlocked;
        PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.ACHIEVEMENTS_COMPLETED] = jsonPlayerStats.iAchievementsCompleted;
    }

    public static void DeseralizeFriends(string friendsJSON)
    {
        JSONFriendList friendList = JsonUtility.FromJson<JSONFriendList>(friendsJSON);

        //Add all the friends into a list
        for (int i = 0; i < friendList.friends.Count; ++i)
        {
            BasicInfo basicInfo = new BasicInfo
            {
                UID = friendList.friends[i].iFriendUID,
                Name = friendList.friends[i].sUsername,
                Unlocked = true
            };
            PlayerData.FriendList.Add(basicInfo);
        }
    }

    public static void DeseralizePhonebookData(string phonebookDataJSON)
    {
        JSONPhonebookDataList phonebookDataList = JsonUtility.FromJson<JSONPhonebookDataList>(phonebookDataJSON);

        //Add all the phonebook data into the dict
        for (int i = 0; i < phonebookDataList.phonebookData.Count; ++i)
        {
            BasicInfo basicInfo = new BasicInfo
            {
                UID = phonebookDataList.phonebookData[i].iOtherUID,
                Name = phonebookDataList.phonebookData[i].sUsername,
                Unlocked = phonebookDataList.phonebookData[i].bOtherUnlocked
            };
            PlayerData.PhonebookData.Add(phonebookDataList.phonebookData[i].iOtherUID, basicInfo);
        }
    }

    public static FriendInfo DeseralizeFriendData(int friendUID, string friendDataJSON)
    {
        JSONFriendDataList friendDataList = JsonUtility.FromJson<JSONFriendDataList>(friendDataJSON);

        JSONFriendData jsonFriendData = friendDataList.friendData[0];

        FriendInfo friendInfo = new FriendInfo();

        //Set all the info needed
        friendInfo.UID = friendUID;
        friendInfo.Name = jsonFriendData.sUsername;
        friendInfo.ShowBirthday = jsonFriendData.bShowBirthday;
        friendInfo.Birthday = jsonFriendData.dBirthday;
        friendInfo.CurrentTitleID = jsonFriendData.iCurrentTitleID;
        friendInfo.Biography = jsonFriendData.sBiography;
        friendInfo.CurrLevel = jsonFriendData.iLevel;
        friendInfo.Department = jsonFriendData.iDepartment;
        friendInfo.GuildID = jsonFriendData.iGuildID;
        friendInfo.Country = jsonFriendData.iCountry;
        friendInfo.FriendshipLevel = jsonFriendData.iFriendshipLevel;
        friendInfo.FriendshipXP = jsonFriendData.iFriendshipXP;

        PlayerData.FriendDataList.Add(friendInfo);
        return friendInfo;
    }

    public static void DeseralizeSentFriendRequests(string sentFriendRequestJSON)
    {
        JSONSentFriendRequestList sentFriendRequestList = JsonUtility.FromJson<JSONSentFriendRequestList>(sentFriendRequestJSON);

        for (int i = 0; i < sentFriendRequestList.sentFriendRequests.Count; ++i)
        {
            PlayerData.SentFriendRequestList.Add(sentFriendRequestList.sentFriendRequests[i].iOtherUID);
        }
    }

    public static void DeseralizeReceivedFriendRequests(string receivedFriendRequestJSON)
    {
        JSONReceivedFriendRequestList receivedFriendRequestList = JsonUtility.FromJson<JSONReceivedFriendRequestList>(receivedFriendRequestJSON);

        for (int i = 0; i < receivedFriendRequestList.receivedFriendRequests.Count; ++i)
        {
            PlayerData.ReceivedFriendRequestList.Add(receivedFriendRequestList.receivedFriendRequests[i].iOwnerUID);
        }
    }

    public static List<JSONAbandonedCity> DeseralizeAbandonedCityInfo(string abandonedCityJSON)
    {
        JSONAbandonedCityList abandonedCityList = JsonUtility.FromJson<JSONAbandonedCityList>(abandonedCityJSON);

        return abandonedCityList.abandonedCities;
    }

    public static JSONGuildInfo DeseralizeGuildInfo(string guildInfoJSON)
    {
        JSONGuildInfoList guildInfoList = JsonUtility.FromJson<JSONGuildInfoList>(guildInfoJSON);

        return guildInfoList.guildInfo[0];
    }

    public static List<JSONRedemptionRequest> DeseralizeRedemptionRequests(string redemptionRequestJSON)
    {
        JSONRedemptionRequestList redemptionRequestList = JsonUtility.FromJson<JSONRedemptionRequestList>(redemptionRequestJSON);

        return redemptionRequestList.redemptionRequests;
    }

    public static Dictionary<Cosmetic, bool> DeseralizeCosmeticData(string cosmeticDataJSON)
    {
        JSONCosmeticDataList cosmeticDataList = JsonUtility.FromJson<JSONCosmeticDataList>(cosmeticDataJSON);

        Dictionary<Cosmetic, bool> cosmeticDataDictionary = new Dictionary<Cosmetic, bool>();

        foreach (JSONCosmeticData cosmeticData in cosmeticDataList.cosmeticData)
        {
            Cosmetic cosmetic = new Cosmetic(cosmeticData.iCosmeticID, cosmeticData.sCosmeticName, cosmeticData.iCosmeticCost, (COSMETIC_RARITY)cosmeticData.iCosmeticRarity, (COSMETIC_TYPE)cosmeticData.iCosmeticType);

            cosmeticDataDictionary.Add(cosmetic, cosmeticData.bCosmeticUnlocked);
        }

        return cosmeticDataDictionary;
    }

    public static List<int> DeseralizeEquippedCosmeticList(string equippedCosmeticListJSON)
    {
        JSONEquippedCosmeticList equippedCosmeticList = JsonUtility.FromJson<JSONEquippedCosmeticList>(equippedCosmeticListJSON);

        List<int> equippedCosmeticIDList = new List<int>();

        for (int i = 0; i < equippedCosmeticList.equippedCosmetics.Count; ++i)
        {
            equippedCosmeticIDList.Add(equippedCosmeticList.equippedCosmetics[i].iCosmeticID);
        }

        return equippedCosmeticIDList;
    }

    public static Dictionary<Title, bool> DeseralizeTitleData(string titleDataJSON)
    {
        JSONTitleDataList titleDataList = JsonUtility.FromJson<JSONTitleDataList>(titleDataJSON);

        Dictionary<Title, bool> titleDictionary = new Dictionary<Title, bool>();

        foreach (JSONTitleData titleData in titleDataList.titleData)
        {
            Title title = new Title(titleData.iTitleID, titleData.sTitleName);

            titleDictionary.Add(title, titleData.bTitleUnlocked);
        }

        return titleDictionary;
    }

    public static Dictionary<Achievement, bool> DeseralizeAchievementData(string achievementDataJSON)
    {
        JSONAchievementDataList achievementDataList = JsonUtility.FromJson<JSONAchievementDataList>(achievementDataJSON);

        Dictionary<Achievement, bool> achievementDataDictionary = new Dictionary<Achievement, bool>();

        foreach (JSONAchievementData achievementData in achievementDataList.achievementData)
        {
            Achievement title = new Achievement(achievementData.iAchievementID, achievementData.sAchievementName, achievementData.sAchievementDescription, achievementData.iEarnedTitleID);

            achievementDataDictionary.Add(title, achievementData.bAchievementClaimed);
        }

        return achievementDataDictionary;
    }

    public static Dictionary<BattleshipInfo, bool> DeseralizeBattleshipData(string battleshipDataJSON)
    {
        JSONBattleshipDataList battleshipDataList = JsonUtility.FromJson<JSONBattleshipDataList>(battleshipDataJSON);

        Dictionary<BattleshipInfo, bool> battleshipDataDictionary = new Dictionary<BattleshipInfo, bool>();

        foreach (JSONBattleshipData battleshipData in battleshipDataList.battleshipData)
        {
            BattleshipInfo battleshipInfo = new BattleshipInfo(battleshipData.iBattleshipID, battleshipData.sBattleshipName, battleshipData.iHP, battleshipData.iAtk, battleshipData.fAtkSpd, battleshipData.fCritRate, battleshipData.fCritDmg, battleshipData.fMoveSpd);

            battleshipDataDictionary.Add(battleshipInfo, battleshipData.bBattleshipUnlocked);
        }

        return battleshipDataDictionary;
    }
}
