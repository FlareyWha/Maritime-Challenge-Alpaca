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
    public static int CurrentBattleship = 1;
    public static Vector3 PlayerPosition;

    public static string activeSubScene = "WorldHubScene";

    public static Player MyPlayer = null;
    public static PlayerCommands CommandsHandler = null;

    //Dict to store friendId
    public static List<BasicInfo> FriendList = new List<BasicInfo>();
    //Dict to store id of people and whether they are unlocked
    public static Dictionary<int, BasicInfo> PhonebookData = new Dictionary<int, BasicInfo>();
    //Dict to store data of friends that have been viewed and saved locally
	public static List<FriendInfo> FriendDataList = new List<FriendInfo>();
    //Stores all the friend requests you have sent to other people
    public static List<int> SentFriendRequestList = new List<int>();
    //Stores all the friend requests you have recieved from other people
    public static List<int> ReceivedFriendRequestList = new List<int>();

    // Stores all the Titles in game, and whether you have unlocked them
    public static Dictionary<Title, bool> TitleDictionary = new Dictionary<Title, bool>();
    // Stores all the cosmetics in game, and whether you have unlocked them
    public static Dictionary<Cosmetic, bool> CosmeticsList = new Dictionary<Cosmetic, bool>();
    // Stores all the equipped cosmetics
    public static List<Cosmetic> EquippedCosmeticsList = new List<Cosmetic>();
    // Stores all the equipped cosmetics
    public static Dictionary<Achievement, bool> AchievementList = new Dictionary<Achievement, bool>();


    public delegate void PlayerDataUpdated();
    public static event PlayerDataUpdated OnPlayerDataUpdated;

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
            if (info.Key == id)
                return info.Value.Name;
        }

        return "Player does not exist";
    }

    public static BasicInfo FindPlayerInfoByID(int id)
    {
        foreach (BasicInfo info in FriendList)
        {
            if (info.UID == id)
                return info;
        }
        return null;
    }

    public static FriendInfo FindFriendInfoByID(int id)
    {
        foreach (FriendInfo info in FriendDataList)
        {
            if (info.UID == id)
                return info;
        }
        return null;
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
        SentFriendRequestList.Clear();
        ReceivedFriendRequestList.Clear();
    }


    public static void SetUsername(string name)
    {
        Name = name;
        OnPlayerDataUpdated?.Invoke();
    }

    public static void SetBio(string bio)
    {
        Biography = bio;
        OnPlayerDataUpdated?.Invoke();
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

