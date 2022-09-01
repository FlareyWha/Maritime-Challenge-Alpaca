using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDataManager : MonoBehaviour
{
    //Server url that will change
    //static string serverURL = "http://localhost/Maritime_Challenge/";
    static string serverURL = "https://ship.focused.lol/Maritime_Challenge/";

    //URL's
    //public static string URL_getUsername = serverURL + "GetUsername.php";
    public static string URL_login = serverURL + "Login.php";
    public static string URL_verifyEmail = serverURL + "VerifyEmail.php";
    public static string URL_register = serverURL + "Register.php";
    public static string URL_getPlayerData = serverURL + "GetPlayerData.php";
    public static string URL_getFriends = serverURL + "GetFriends.php";
    public static string URL_getPhonebookData = serverURL + "GetPhonebookData.php";
    public static string URL_setOnline = serverURL + "SetOnline.php";
    public static string URL_addFriend = serverURL + "AddFriend.php";
    public static string URL_deleteFriend = serverURL + "DeleteFriend.php";
    public static string URL_getFriendInfo = serverURL + "GetFriendInfo.php";
    public static string URL_updateAccountXPLevels = serverURL + "UpdateAccountXPLevels.php";
    public static string URL_updateTotalRightshipRollars = serverURL + "UpdateTotalRightshipRollars.php";
    public static string URL_updateTotalTokens = serverURL + "UpdateTotalTokens.php";
    public static string URL_updateTotalEventCurrency = serverURL + "UpdateTotalEventCurrency.php";
}
