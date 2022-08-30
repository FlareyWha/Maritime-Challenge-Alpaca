using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static int UID = 0;
    public static string Name = "Unset";
    public static bool ShowBirthday = true;
    public static string Birthday = "0000-0-0";
    public static int CurrentTitleID = 1;
    public static string Biography = "";
    public static int CurrLevel = 0;
    public static int CurrXP = 0;
    public static int Department = 0;
    public static int Guild = 0;
    public static int Country = 0;
    public static int NumRightshipRollers = 0;
    public static int NumTokens = 0;
    public static int NumEventCurrency = 0;
    public static Vector3 PlayerPosition;

    //Dict to store friendId and their friendship level
    public static Dictionary<int, int> FriendList = new Dictionary<int, int>();
    //Dict to store id of people and whether they are unlocked
    public static Dictionary<int, bool> PhonebookData = new Dictionary<int, bool>();


    public static Player MyPlayer = null;
}
