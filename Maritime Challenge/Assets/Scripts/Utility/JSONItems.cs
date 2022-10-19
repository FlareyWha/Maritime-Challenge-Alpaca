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
    public string dtLastLogin;
}

[Serializable]
public class JSONPlayerDataList
{
    public List<JSONPlayerData> playerData = new List<JSONPlayerData>();
}

[Serializable]
public class JSONPlayerStats
{
    public int iEnemiesDefeated;
    public int iBossesDefeated;
    public int iFriendsAdded;
    public int iRightshipediaEntriesUnlocked;
    public int iBattleshipsOwned;
    public int iCosmeticsOwned;
    public int iTitlesUnlocked;
    public int iAchievementsCompleted;
}

[Serializable]
public class JSONPlayerStatsList
{
    public List<JSONPlayerStats> playerStats = new List<JSONPlayerStats>();
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
    public int iFriendshipXP;
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
public class JSONEquippedCosmetic
{
    public int iCosmeticID;
}

[Serializable]
public class JSONEquippedCosmeticList
{
    public List<JSONEquippedCosmetic> equippedCosmetics = new List<JSONEquippedCosmetic>();
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

[Serializable]
public class JSONAchievementData
{
    public int iAchievementID;
    public string sAchievementName;
    public string sAchievementDescription;
    public int iEarnedTitleID;
    public bool bAchievementClaimed;
}

[Serializable]
public class JSONAchievementDataList
{
    public List<JSONAchievementData> achievementData = new List<JSONAchievementData>();
}

[Serializable]
public class JSONMissionData
{
    public int iMissionID;
    public string sMissionName;
    public int iMissionType;
    public int iMissionRequirementMaxNumber;
    public bool bMissionClaimed;
}

[Serializable]
public class JSONMissionDataList
{
    public List<JSONMissionData> missionData = new List<JSONMissionData>();
}

[Serializable]
public class JSONBattleshipData
{
    public int iBattleshipID;
    public string sBattleshipName;
    public int iHP;
    public int iAtk;
    public int fAtkSpd;
    public int fCritRate;
    public int fCritDmg;
    public int fMoveSpd;
    public bool bBattleshipUnlocked;
}

[Serializable]
public class JSONBattleshipDataList
{
    public List<JSONBattleshipData> battleshipData = new List<JSONBattleshipData>();
}

[Serializable]
public class JSONMailData
{
    public int iMailID;
    public string sMailTitle;
    public string sMailDescription;
    public int iMailItemAmount;
}

[Serializable]
public class JSONMailDataList
{
    public List<JSONMailData> mailData = new List<JSONMailData>();
}
