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
        PlayerData.Birthday = jsonPlayerData.dBirthday;
        PlayerData.CurrentTitleID = jsonPlayerData.iCurrentTitleID;
        PlayerData.Biography = jsonPlayerData.sBiography;
        PlayerData.CurrLevel = jsonPlayerData.iLevel;
        PlayerData.CurrXP = jsonPlayerData.iXP;
        PlayerData.Department = jsonPlayerData.iDepartment;
        PlayerData.Guild = jsonPlayerData.iGuildID;
        PlayerData.Country = jsonPlayerData.iCountry;
        PlayerData.NumRightshipRollers = jsonPlayerData.iTotalRightshipRollers;
        PlayerData.NumTokens = jsonPlayerData.iTotalTokens;
        PlayerData.NumEventCurrency = jsonPlayerData.iTotalEventCurrency;
        PlayerData.PlayerPosition = new Vector3(jsonPlayerData.fPlayerXPos, jsonPlayerData.fPlayerYPos, jsonPlayerData.fPlayerZPos);
    }

    public static void DeseralizeFriends(string friendsJSON)
    {
        JSONFriendList friendList = JsonUtility.FromJson<JSONFriendList>(friendsJSON);

        //Add all the friends into a list
        for (int i = 0; i < friendList.friends.Count; ++i)
        {
            PlayerData.FriendList.Add(friendList.friends[i].iFriendUID, friendList.friends[i].iFriendshipLevel);
        }
    }

    public static void DeseralizePhonebookData(string phonebookDataJSON)
    {
        JSONPhonebookDataList phonebookDataList = JsonUtility.FromJson<JSONPhonebookDataList>(phonebookDataJSON);

        //Add all the phonebook data into the dict
        for (int i = 0; i < phonebookDataList.phonebookData.Count; ++i)
        {
            PlayerData.PhonebookData.Add(phonebookDataList.phonebookData[i].iOtherUID, phonebookDataList.phonebookData[i].bOtherUnlocked);
        }
    }
}
