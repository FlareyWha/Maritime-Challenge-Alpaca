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
    ENEMIES_DEFEATED,
    BOSSES_DEFEATED,
    FRIENDS_ADDED,
    RIGHTSHIPEDIA_ENTRIES_UNLOCKED,
    BATTLESHIPS_OWNED,
    COSMETICS_OWNED,
    TITLES_UNLOCKED,
    ACHIEVEMENTS_COMPLETED,
    NO_PLAYER_STAT
}
