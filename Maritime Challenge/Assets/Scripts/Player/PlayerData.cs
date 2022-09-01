using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData // Local Player's Data
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
    public static int GuildID = 0;
    public static int Country = 0;
    public static int NumRightshipRollars = 0;
    public static int NumTokens = 0;
    public static int NumEventCurrency = 0;
    public static Vector3 PlayerPosition;

    // List<FriendInfo> AllUnlockedList 

    //Dict to store friendId
    public static List<BasicInfo> FriendList = new List<BasicInfo>();
    //Dict to store id of people and whether they are unlocked
    public static Dictionary<BasicInfo, bool> PhonebookData = new Dictionary<BasicInfo, bool>();

    public static Player MyPlayer = null;

	public static List<FriendInfo> FriendDataList = new List<FriendInfo>();


    public static string GetCountryName(int id)
    {
        switch (id)
        {
            case 0:
                return "Singapore";
            case 1:
                return "America";
            case 2:
                return "Australia";
            case 3:
                return "Europe";
        }
        return "Country does not exist";
    }

    public static Sprite GetTitleByID(int id)
    {
        return null;
    }

   

}

public class BasicInfo
{
    public int UID;
    public string Name;
    //public List<Cosmetic> Cosmetics;
}

public class FriendInfo : BasicInfo
{
    public bool ShowBirthday;
    public string Birthday;
    public int CurrentTitleID;
    public string Biography;
    public int CurrLevel;
    public int Department;
    public int GuildID;
    public int Country;
    public bool Online;
    public int FriendshipLevel;
}

