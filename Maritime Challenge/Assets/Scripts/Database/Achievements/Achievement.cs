using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement
{
    public int AchievementID;
    public string AchievementName;
    public string AchievementDescription;
    public int EarnedTitleID;
    public int RightshipRollarsEarned;

    public AchievementSO AchievementData;


    public Achievement(int ID, string name, string description, int titleID, int rightshipRollarsEarned)
    {
        AchievementID = ID;
        AchievementName = name;
        AchievementDescription = description;
        EarnedTitleID = titleID;
        RightshipRollarsEarned = rightshipRollarsEarned;
    }
}