using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsUI : MonoBehaviour
{
    [SerializeField]
    private Text MissionTitle;
    [SerializeField]
    private Button ClaimButton;
    [SerializeField]
    private Image MissionProgressFill;
    [SerializeField]
    private Text MissionProgressText;

    private int missionID = 0;

    public void Init()
    {
        
    }

    public int GetMissionID()
    {
        return missionID;
    }
}
