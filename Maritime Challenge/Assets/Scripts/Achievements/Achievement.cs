using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement
{
    public int AchievementID;
    public string AchievementName;
    public string AchievementDescription;
    public int AchievementRequirementMaxNumber;
    public int EarnedTitleID;

    public Achievement(int ID, string name, string description, int requirementMaxNumber, int titleID)
    {
        AchievementID = ID;
        AchievementName = name;
        AchievementDescription = description;
        AchievementRequirementMaxNumber = requirementMaxNumber;
        EarnedTitleID = titleID;
    }
}

public enum ACHIEVEMENT_ID
{
    

}