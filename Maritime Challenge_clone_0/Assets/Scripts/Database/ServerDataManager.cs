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

    #region Handling Player Data

    public static string URL_getPlayerData = serverURL + "GetPlayerData.php";

    //Game data
    public static string URL_updateAccountXPLevels = serverURL + "UpdateAccountXPLevels.php";
    public static string URL_updateTotalRightshipRollars = serverURL + "UpdateTotalRightshipRollars.php";
    public static string URL_updateTotalTokens = serverURL + "UpdateTotalTokens.php";
    public static string URL_updateTotalEventCurrency = serverURL + "UpdateTotalEventCurrency.php";

    //Profile data
    public static string URL_updateUsername = serverURL + "UpdateUsername.php";
    public static string URL_updateBiography = serverURL + "UpdateBiography.php";
    public static string URL_updateTitle = serverURL + "UpdateTitle.php";
    public static string URL_updateOnlineStatus = serverURL + "UpdateOnlineStatus.php";
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
}
