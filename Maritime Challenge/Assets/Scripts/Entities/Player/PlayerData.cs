using System.Collections;
using System.Collections.Generic;
using System;
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
    public static DateTime LastLogin;

    public static PlayerStats PlayerStats = new PlayerStats();

    public static string activeSubScene = "WorldHubScene";

    public static Player MyPlayer = null;
    public static PlayerCommands CommandsHandler = null;

    //Dict to store friendId
    public static List<BasicInfo> FriendList = new List<BasicInfo>();
    //Dict to store id of people and whether they are unlocked
    public static Dictionary<int, BasicInfo> PhonebookData = new Dictionary<int, BasicInfo>();
    //Dict to store the equipped cosmetics of others
    public static Dictionary<int, List<Cosmetic>> OthersEquippedCosmeticList = new Dictionary<int, List<Cosmetic>>();
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
    // Stores all the achievements and whether they are claimed
    public static Dictionary<Achievement, bool> AchievementList = new Dictionary<Achievement, bool>();
    // Stores all the missions and whether they are claimed
    public static Dictionary<Mission, bool> MissionList = new Dictionary<Mission, bool>();
    // Stores all the battleships in the game, and whether you have unlocked them
    public static Dictionary<BattleshipInfo, bool> BattleshipList = new Dictionary<BattleshipInfo, bool>();
    // Stores all the mail that you have
    public static List<Mail> MailList = new List<Mail>();


    public delegate void PlayerDataUpdated();
    public static event PlayerDataUpdated OnPlayerDataUpdated;

    public delegate void PlayerStatsUpdated();
    public static event PlayerStatsUpdated OnPlayerStatsUpdated;

    public delegate void NumTokensUpdated();
    public static event NumTokensUpdated OnNumTokensUpdated;

    public delegate void NumRollarsUpdated();
    public static event NumRollarsUpdated OnNumRollarsUpdated;

    public static int GetCurrentProgressNum(PLAYER_STAT type)
    {
        return PlayerStats.PlayerStat[(int)type];

    }
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
        return "-";
    }

    public static string GetGuildName(int id)
    {
        switch (id)
        {
            case 1:
                return "Guild1";
            case 2:
                return "Guild2";
            case 3:
                return "Guild3";
            case 4:
                return "Guild4";
        }
        return "-";
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
        foreach (KeyValuePair<int, BasicInfo> info in PhonebookData)
        {
            if (info.Key == id)
                return info.Value;
        }
        return null;
    }

    public static BasicInfo FindPlayerFromFriendList(int id)
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

    public static BattleshipInfo FindBattleshipInfoByID(int ID)
    {
        foreach (KeyValuePair<BattleshipInfo, bool> shipData in BattleshipList)
        {
            if (shipData.Key.BattleshipID == ID)
                return shipData.Key;
        }

        Debug.LogWarning("PlayerData: Could not Find BattleshipInfo of ID " + ID);
        return null;

    }


    public static void CheckForDailyReset()
    {
        DateTime CurrentDateTime = System.DateTime.UtcNow;
        DateTime ResetDateTime = new DateTime(CurrentDateTime.Year, CurrentDateTime.Month, CurrentDateTime.Day,
            GameSettings.DailyResetTime.Hour, GameSettings.DailyResetTime.Minute, GameSettings.DailyResetTime.Second);

        if (LastLogin < ResetDateTime && CurrentDateTime > ResetDateTime)
        {
            // Reset Stats

            // 
        }
    }

    public static void InitGuestData()
    {
        UID = 0;
        Name = "Guest";
        ShowBirthday = false;
        //Birthday = "0000-0-0";
        CurrentTitleID = 1;
        Biography = "";
        CurrLevel = 0;
        CurrXP = 0;
        Department = 0;
        GuildID = -1;
        Country = -1;
        NumRightshipRollars = 0;
        NumTokens = 0;
        NumEventCurrency = 0;
        PlayerPosition = Vector3.zero;
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
        GuildID = -1;
        Country = -1;
        NumRightshipRollars = 0;
        NumTokens = 0;
        NumEventCurrency = 0;
        PlayerPosition = Vector3.zero;

        PhonebookData.Clear();
        FriendList.Clear();
        FriendDataList.Clear();
        SentFriendRequestList.Clear();
        ReceivedFriendRequestList.Clear();
        AchievementList.Clear();
        MissionList.Clear();
        CosmeticsList.Clear();
        OthersEquippedCosmeticList.Clear();
        TitleDictionary.Clear();
        BattleshipList.Clear();
        MailList.Clear();
    }

    public static void SetAchievementClaimed(Achievement achvment)
    {
        AchievementList[achvment] = true;
    }
    public static void SetMissionClaimed(Mission mission)
    {
        MissionList[mission] = true;
    }
    public static void SetTitleUnlocked(Title title)
    {
        TitleDictionary[title] = true;
    }
    public static void SetBattleshipUnlocked(BattleshipInfo shipInfo)
    {
        BattleshipList[shipInfo] = true;
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

    public static void InvokePlayerStatsUpdated()
    {
        OnPlayerStatsUpdated.Invoke();
    }

    public static void InvokeNumTokensUpdated()
    {
        OnNumTokensUpdated.Invoke();
    }

    public static void InvokeNumRollarsUpdated()
    {
        OnNumRollarsUpdated.Invoke();
    }

}

public class BasicInfo
{
    public int UID;
    public string Name;
    public bool Unlocked;
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
    public int FriendshipXP;
}

