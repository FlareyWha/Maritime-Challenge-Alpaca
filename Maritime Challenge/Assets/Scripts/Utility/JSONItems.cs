using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class JSONPlayerData
{
    public string sUsername;
    public bool bShowBirthday;
    public string dBirthday;
    public int iCurrentTitleID;
    public string sBiography;
    public int iLevel;
    public int iXP;
    public int iDepartment;
    public int iGuildID;
    public int iCountry;
    public int iTotalRightshipRollars;
    public int iTotalTokens;
    public int iTotalEventCurrency;
    public int iCurrentBattleship;
    public float fPlayerXPos;
    public float fPlayerYPos;
    public float fPlayerZPos;
}

[Serializable]
public class JSONPlayerDataList
{
    public List<JSONPlayerData> playerData = new List<JSONPlayerData>();
}

[Serializable]
public class JSONFriends
{
    public int iFriendUID;
    public string sUsername;
}

[Serializable]
public class JSONFriendList
{
    public List<JSONFriends> friends = new List<JSONFriends>();
}

[Serializable]
public class JSONPhonebookData
{
    public int iOtherUID;
    public string sUsername;
    public bool bOtherUnlocked;
}

[Serializable]
public class JSONPhonebookDataList
{
    public List<JSONPhonebookData> phonebookData = new List<JSONPhonebookData>();
}

[Serializable]
public class JSONFriendData
{
    public string sUsername;
    public bool bShowBirthday;
    public string dBirthday;
    public int iCurrentTitleID;
    public string sBiography;
    public int iLevel;
    public int iXP;
    public int iDepartment;
    public int iGuildID;
    public int iCountry;
    public int iFriendshipLevel;
}

[Serializable]
public class JSONFriendDataList
{
    public List<JSONFriendData> friendData = new List<JSONFriendData>();
}

[Serializable]
public class JSONSentFriendRequest
{
    public int iOtherUID;
}

[Serializable]
public class JSONSentFriendRequestList
{
    public List<JSONSentFriendRequest> sentFriendRequests = new List<JSONSentFriendRequest>();
}

[Serializable]
public class JSONReceivedFriendRequest
{
    public int iOwnerUID;
}

[Serializable]
public class JSONReceivedFriendRequestList
{
    public List<JSONReceivedFriendRequest> receivedFriendRequests = new List<JSONReceivedFriendRequest>();
}

[Serializable]
public class JSONAbandonedCity
{
    public int iAbandonedCityID;
    public string sAbandonedCityName;
    public int iAbandonedCityAreaCellWidth;
    public int iAbandonedCityAreaCellHeight;
    public int fAbandonedCityXPos;
    public int fAbandonedCityYPos;
    public int iCapturedGuildID;
}

[Serializable]
public class JSONAbandonedCityList
{
    public List<JSONAbandonedCity> abandonedCities = new List<JSONAbandonedCity>();
}

[Serializable]
public class JSONGuildInfo
{
    public string sGuildName;
    public string sGuildDescription;
    public int iOwnerUID;
}

[Serializable]
public class JSONGuildInfoList
{
    public List<JSONGuildInfo> guildInfo = new List<JSONGuildInfo>();
}

[Serializable]
public class JSONRedemptionRequest
{
    public string sUsername;
    public string sRedemptionItemName;
}

[Serializable]
public class JSONRedemptionRequestList
{
    public List<JSONRedemptionRequest> redemptionRequests = new List<JSONRedemptionRequest>();
}

[Serializable]
public class JSONCosmeticData
{
    public int iCosmeticID;
    public string sCosmeticName;
    public int iCosmeticCost;
    public int iCosmeticRarity;
    public int iCosmeticType;
    public bool bCosmeticUnlocked;
}

[Serializable]
public class JSONCosmeticDataList
{
    public List<JSONCosmeticData> cosmeticData = new List<JSONCosmeticData>();
}

[Serializable]
public class JSONTitleData
{
    public int iTitleID;
    public string sTitleName;
    public bool bTitleUnlocked;
}

[Serializable]
public class JSONTitleDataList
{
    public List<JSONTitleData> titleData = new List<JSONTitleData>();
}