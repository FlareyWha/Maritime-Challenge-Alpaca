using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDataManager : MonoBehaviourSingleton<ServerDataManager>
{
    //Server url that will change
    static string serverURL = "http://localhost/Maritime_Challenge/";
    //static string serverURL = "http://ec2-107-20-224-130.compute-1.amazonaws.com/lab02_daevon_200412T/";

    //URL's
    public string URL_getUID = serverURL + "GetUid.php";
    public string URL_getUsername = serverURL + "GetUsername.php";
    public string URL_login = serverURL + "Login.php";
    public string URL_verifyEmail = serverURL + "VerifyEmail.php";
    public string URL_register = serverURL + "Register.php";
}
