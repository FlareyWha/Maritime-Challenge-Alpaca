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

    //Login & Register
    public static string URL_login = serverURL + "Login.php";
    public static string URL_verifyEmail = serverURL + "VerifyEmail.php";
    public static string URL_register = serverURL + "Register.php";

    #endregion

    #region Handling Player Data & Cosmetics

    public static string URL_getPlayerData = serverURL + "GetPlayerData.php";

    //Game data
    public static string URL_updateAccountXPLevels = serverURL + "UpdateAccountXPLevels.php";
    public static string URL_updateTotalRightshipRollars = serverURL + "UpdateTotalRightshipRollars.php";
    public static string URL_updateTotalTokens = serverURL + "UpdateTotalTokens.php";
    public static string URL_updateTotalEventCurrency = serverURL + "UpdateTotalEventCurrency.php";

    //Profile data
    public static string URL_getUsername = serverURL + "GetUsername.php";
    public static string URL_updateUsername = serverURL + "UpdateUsername.php";
    public static string URL_updateBiography = serverURL + "UpdateBiography.php";
    public static string URL_updateTitle = serverURL + "UpdateTitle.php";

    //Cosmetics
    public static string URL_getCosmeticData = serverURL + "GetCosmeticData.php";

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

    #endregion

    #region Handling Phonebook Data

    public static string URL_updatePhonebookOtherUnlocked = serverURL + "UpdatePhonebookOtherUnlocked.php";
    public static string URL_getPhonebookData = serverURL + "GetPhonebookData.php";

    #endregion

    #region Handling Missions & Achievements

    public static string URL_getMissionStatus = serverURL + "GetMissionStatus.php";
    public static string URL_getAchievementStatus = serverURL + "GetAchievementStatus.php";

    #endregion

    #region Handling Guilds & Abandoned Cities

    public static string URL_getGuildInfo = serverURL + "GetGuildInfo.php";

    public static string URL_getAbandonedCityInfo = serverURL + "GetAbandonedCityInfo.php";
    public static string URL_updateAbandonedCityCapturedGuildID = serverURL + "UpdateAbandonedCityCapturedGuildID.php";

    #endregion

    #region Handling Admin Stuff

    public static string URL_getRedemptionRequests = serverURL + "GetRedemptionRequests.php";

    #endregion
}
