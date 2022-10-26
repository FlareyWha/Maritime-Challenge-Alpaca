using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission
{
    public int MissionID;
    public string MissionName;
    public int MissionType;
    public int MissionRequirementMaxNumber;
    public int TokensEarned;

    public MissionSO MissionData;

    public Mission(int missionID, string missionName, int missionType, int missionRequirementMaxNumber, int tokensEarned)
    {
        MissionID = missionID;
        MissionName = missionName;
        MissionType = missionType;
        MissionRequirementMaxNumber = missionRequirementMaxNumber;
        TokensEarned = tokensEarned;
    }
}

public enum MISSION_TYPE
{
    DAILY,
    WEEKLY,
    SEASONAL,

    NUM_MISSION_TYPE
}
