using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDataManager : MonoBehaviour
{
    //Server url that will change
    //static string serverURL = "http://localhost/Maritime_Challenge/";
    static string serverURL = "https://ship.focused.lol/Maritime_Challenge/";

    //URL's

    #region Handling Login & Register

    //Login
    public static string URL_login = serverURL + "Login.php";

    //Register
    public static string URL_verifyEmail = serverURL + "VerifyEmail.php";
    public static string URL_register = serverURL + "Register.php";
    public static string URL_addPlayerStats = serverURL + "AddPlayerStats.php";
    public static string URL_updateDefaultUnlockedCosmetics = serverURL + "UpdateDefaultUnlockedCosmetics.php";
    public static string URL_addDefaultEquippedCosmetics = serverURL + "AddDefaultEquippedCosmetics.php";

    #endregion

    #region Handling Player Data & Stats & Cosmetics

    public static string URL_getPlayerData = serverURL + "GetPlayerData.php";
    public static string URL_getPlayerStats = serverURL + "GetPlayerStats.php";
    public static string URL_updatePlayerStats = serverURL + "UpdatePlayerStats.php";
    public static string URL_updateLastLoginTime = serverURL + "UpdateLastLoginTime.php";

    //Game data
    public static string URL_updateAccountXPLevels = serverURL + "UpdateAccountXPLevels.php";
    public static string URL_getBattleshipData = serverURL + "GetBattleshipData.php";
    public static string URL_updateBattleshipList = serverURL + "UpdateBattleshipList.php";
    public static string URL_updateCurrentBattleship = serverURL + "UpdateCurrentBattleship.php";
    public static string URL_updateTotalRightshipRollars = serverURL + "UpdateTotalRightshipRollars.php";
    public static string URL_updateTotalTokens = serverURL + "UpdateTotalTokens.php";
    public static string URL_updateTotalEventCurrency = serverURL + "UpdateTotalEventCurrency.php";

    //Profile data
    public static string URL_getUsername = serverURL + "GetUsername.php";
    public static string URL_updateUsername = serverURL + "UpdateUsername.php";
    public static string URL_updateBiography = serverURL + "UpdateBiography.php";
    public static string URL_getTitleData = serverURL + "GetTitleData.php";
    public static string URL_updateCurrentTitle = serverURL + "UpdateCurrentTitle.php";
    public static string URL_updateTitleList = serverURL + "UpdateTitleList.php";

    //Cosmetics
    public static string URL_getCosmeticData = serverURL + "GetCosmeticData.php";
    public static string URL_updateCosmeticList = serverURL + "UpdateCosmeticList.php";
    public static string URL_getEquippedCosmeticList = serverURL + "GetEquippedCosmeticList.php";
    public static string URL_updateEquippedCosmeticList = serverURL + "UpdateEquippedCosmeticList.php";


    #endregion

    #region Handling Friends & Friend Requests

    public static string URL_addFriend = serverURL + "AddFriend.php";
    public static string URL_addFriendRequest = serverURL + "AddFriendRequest.php";

    public static string URL_getFriendInfo = serverURL + "GetFriendInfo.php";
    public static string URL_getFriends = serverURL + "GetFriends.php";
    public static string URL_getSentFriendRequests = serverURL + "GetSentFriendRequests.php";
    public static string URL_getReceivedFriendRequests = serverURL + "GetReceivedFriendRequests.php";

    public static string URL_deleteFriend = serverURL + "DeleteFriend.php";
    public static string URL_deleteFriendRequest = serverURL + "DeleteFriendRequest.php";

    //Friendship Points
    public static string URL_updateFriendshipXPLevels = "UpdateFriendshipXPLevels.php";

    #endregion

    #region Handling Phonebook Data

    public static string URL_updatePhonebookOtherUnlocked = serverURL + "UpdatePhonebookOtherUnlocked.php";
    public static string URL_getPhonebookData = serverURL + "GetPhonebookData.php";

    #endregion

    #region Handling Missions & Achievements

    public static string URL_getMissionData = serverURL + "GetMissionData.php";
    public static string URL_updateMissionClaimed = serverURL + "UpdateMissionClaimed.php";
    public static string URL_getAchievementData = serverURL + "GetAchievementData.php";
    public static string URL_updateAchievementClaimed = serverURL + "UpdateAchievementClaimed.php";

    #endregion

    #region Handling Guilds & Abandoned Cities

    public static string URL_getGuildInfo = serverURL + "GetGuildInfo.php";

    public static string URL_getAbandonedCityInfo = serverURL + "GetAbandonedCityInfo.php";
    public static string URL_updateAbandonedCityCapturedGuildID = serverURL + "UpdateAbandonedCityCapturedGuildID.php";

    #endregion

    #region Handling Mail

    //Mail
    public static string URL_addMail = serverURL + "AddMail.php";
    public static string URL_deleteMail = serverURL + "DeleteMail.php";
    public static string URL_getMailData = serverURL + "GetMailData.php";

    #endregion

    #region Handling Admin Stuff

    //Redemptions
    public static string URL_getRedemptionRequests = serverURL + "GetRedemptionRequests.php";

    //Refresh database
    public static string URL_addAchievementListData = serverURL + "AddAchievementListData.php";
    public static string URL_addMissionListData = serverURL + "AddMissionListData.php";
    public static string URL_addBattleshipListData = serverURL + "AddBattleshipListData.php";
    public static string URL_addCosmeticListData = serverURL + "AddCosmeticListData.php";
    public static string URL_addTitleListData = serverURL + "AddTitleListData.php";

    #endregion
}
