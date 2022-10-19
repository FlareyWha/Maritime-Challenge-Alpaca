using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerStats
{
    public int[] PlayerStat = new int[(int)PLAYER_STAT.NO_PLAYER_STAT];
    public readonly string[] statNames = { "iEnemiesDefeated", "iBossesDefeated", "iFriendsAdded", "iRightshipediaEntriesUnlocked", "iBattleshipsOwned", "iCosmeticsOwned", "iTitlesUnlocked", "iAchievementsCompleted" };

  
}

public enum PLAYER_STAT
{
    ENEMIES_DEFEATED = 0,
    BOSSES_DEFEATED = 1,
    FRIENDS_ADDED = 2,
    RIGHTSHIPEDIA_ENTRIES_UNLOCKED = 3,
    BATTLESHIPS_OWNED = 4,
    COSMETICS_OWNED = 5,
    TITLES_UNLOCKED = 6,
    ACHIEVEMENTS_COMPLETED = 7,

    NO_PLAYER_STAT
}



