using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviourSingleton<DataManager>
{
    //Info that would need to be stored
    private int uid;
    public int UID
    {
        get { return uid; }
        set
        {
            if (value > 0)
                uid = value;
        }
    }

    //Server url that will change
    static string serverURL = "http://localhost/Maritime_Challenge/";
    //static string serverURL = "http://ec2-107-20-224-130.compute-1.amazonaws.com/lab02_daevon_200412T/";

    //URL's
    public string URL_getUid = serverURL + "get_uid.php";
    public string URL_getUsername = serverURL + "get_username.php";
    public string URL_login = serverURL + "login.php";
    public string URL_verifyEmail = serverURL + "verify_email.php";
    public string URL_register = serverURL + "register.php";
}
