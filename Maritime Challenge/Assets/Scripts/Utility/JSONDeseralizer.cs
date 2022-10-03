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

    public static List<Cosmetic> DeseralizeCosmeticData(string cosmeticDataJSON)
    {
        JSONCosmeticDataList cosmeticDataList = JsonUtility.FromJson<JSONCosmeticDataList>(cosmeticDataJSON);

        List<Cosmetic> cosmeticList = new List<Cosmetic>();

        foreach (JSONCosmeticData cosmeticData in cosmeticDataList.cosmeticData)
        {
            Cosmetic cosmetic = new Cosmetic(cosmeticData.iCosmeticID, cosmeticData.sCosmeticName, cosmeticData.iCosmeticCost, (CosmeticRarity)cosmeticData.iCosmeticRarity, (AvatarBodyPartType)cosmeticData.iCosmeticType);

            cosmeticList.Add(cosmetic);
        }

        return cosmeticList;
    }

    public static Dictionary<int, bool> DeseralizeCosmeticStatusList(string cosmeticStatusListJSON)
    {
        JSONCosmeticStatusList jsonCosmeticStatusList = JsonUtility.FromJson<JSONCosmeticStatusList>(cosmeticStatusListJSON);

        Dictionary<int, bool> cosmeticStatusList = new Dictionary<int, bool>();

        foreach (JSONCosmeticStatus cosmeticStatus in jsonCosmeticStatusList.cosmeticStatusList)
        {
            cosmeticStatusList.Add(cosmeticStatus.iCosmeticID, cosmeticStatus.bCosmeticUnlocked);
        }

        return cosmeticStatusList;
    }
}
