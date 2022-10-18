using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission
{
    public int MissionID;
    public string MissionName;
    public string MissionDescription;

    public MissionSO MissionData;

    public Mission(int missionID, string missionName, string missionDescription)
    {
        MissionID = missionID;
        MissionName = missionName;
        MissionDescription = missionDescription;
    }
}

public enum MISSION_TYPE
{
    DAILY,
    WEEKLY,
    SEASONAL,

    NUM_MISSION_TYPE
}
