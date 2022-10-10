using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement
{
    public int AchievementID;
    public string AchievementName;
    public string AchievementDescription;
    public int EarnedTitleID;

    public AchievementSO AchievementData;


    public Achievement(int ID, string name, string description, int titleID)
    {
        AchievementID = ID;
        AchievementName = name;
        AchievementDescription = description;
        EarnedTitleID = titleID;
    }
}


public enum ACHIEVEMENT_TYPE
{
    FRIENDS,
    RIGHTSHIPEDIA,
    BATTLESHIPS_OWNED,

    NUM_ACHIEVEMENT_TYPE
}
