using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDataManager : MonoBehaviour
{
    //Server url that will change
    static string serverURL = "http://localhost/Maritime_Challenge/";
    //static string serverURL = "https://ship.focused.lol/Maritime_Challenge/";

    //URL's
    public static string URL_getUsername = serverURL + "GetUsername.php";
    public static string URL_login = serverURL + "Login.php";
    public static string URL_verifyEmail = serverURL + "VerifyEmail.php";
    public static string URL_register = serverURL + "Register.php";
    public static string URL_getPlayerData = serverURL + "GetPlayerData.php";
}
