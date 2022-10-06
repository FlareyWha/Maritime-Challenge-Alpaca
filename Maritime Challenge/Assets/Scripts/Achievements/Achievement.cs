using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement
{
    public int AchievementID;
    public string AchievementName;
    public string AchievementDescription;
    public int AchievementTier;
    public int AchievementRequirementMaxNumber;
    public int EarnedTitleID;

    public Achievement(int ID, string name, string description, int tier, int requirementMaxNumber, int titleID)
    {
        AchievementID = ID;
        AchievementName = name;
        AchievementDescription = description;
        AchievementTier = tier;
        AchievementRequirementMaxNumber = requirementMaxNumber;
        EarnedTitleID = titleID;
    }
}
