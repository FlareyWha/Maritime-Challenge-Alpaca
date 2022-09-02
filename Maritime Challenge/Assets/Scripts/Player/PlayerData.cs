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


    public static Player MyPlayer = null;

    //Dict to store friendId
    public static List<BasicInfo> FriendList = new List<BasicInfo>();
    //Dict to store id of people and whether they are unlocked
    public static Dictionary<int, BasicInfo> PhonebookData = new Dictionary<int, BasicInfo>();

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

    public static string GetGuildName(int id)
    {
        switch (id)
        {
            case 0:
                return "Guild1";
            case 1:
                return "Guild2";
            case 2:
                return "Guild3";
            case 3:
                return "Guild4";
        }
        return "Guild does not exist";
    }

    public static Sprite GetTitleByID(int id)
    {
        return null;
    }

    public static string FindPlayerNameByID(int id)
    {
        foreach (KeyValuePair<int, BasicInfo> info in PhonebookData)
        {
            if (info.Value.UID == id)
                return info.Value.Name;
        }

        return "Player does not exist";
    }

    public static void ResetData()
    {
        UID = 0;
        Name = "Unset";
        ShowBirthday = true;
        Birthday = "0000-0-0";
        CurrentTitleID = 1;
        Biography = "";
        CurrLevel = 0;
        CurrXP = 0;
        Department = 0;
        GuildID = 0;
        Country = 0;
        NumRightshipRollars = 0;
        NumTokens = 0;
        NumEventCurrency = 0;
        PlayerPosition = Vector3.zero;

        FriendList.Clear();
        PhonebookData.Clear();
        FriendDataList.Clear();
    }

}

public class BasicInfo
{
    public int UID;
    public string Name;
    public bool Unlocked;
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

