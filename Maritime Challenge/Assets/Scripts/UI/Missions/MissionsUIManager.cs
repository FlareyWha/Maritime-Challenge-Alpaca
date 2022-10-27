using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject MissionsUIPrefab;
    [SerializeField]
    private Transform DailyMissionsRect, WeeklyMissionsRect, SeasonalMissionsRect;


    private void Start()
    {
        UpdateMissionsRect();
        PlayerData.OnPlayerStatsUpdated += OnPlayerStatsUpdated;
    }

    private void OnDestroy()
    {
        PlayerData.OnPlayerStatsUpdated -= OnPlayerStatsUpdated;
    }
    private void UpdateMissionsRect()
    {
        // Clear List
        foreach (Transform child in DailyMissionsRect)
            Destroy(child.gameObject);
        foreach (Transform child in WeeklyMissionsRect)
            Destroy(child.gameObject);
        foreach (Transform child in SeasonalMissionsRect)
            Destroy(child.gameObject);
        // Instantiate New UI GOs
        List<MissionsUI> missionList = new List<MissionsUI>();
        foreach (KeyValuePair<Mission, bool> mission in PlayerData.MissionList)
        {
            MissionsUI missionUI = Instantiate(MissionsUIPrefab).GetComponent<MissionsUI>();
            int currentNum = PlayerData.GetCurrentProgressNum(mission.Key.MissionData.StatType);
            int reqNum = mission.Key.MissionRequirementMaxNumber;
            missionUI.Init(mission.Key, currentNum, reqNum, ClaimCompletedMission);
            if (mission.Value)
            {
                missionUI.SortOrderRef = -1;
                missionUI.SetCompleted();
            }
            else if (currentNum >= reqNum)
                missionUI.SortOrderRef = 1;
            else
                missionUI.SortOrderRef = 0;
            missionList.Add(missionUI);
        }
        // Sort and Add to Rect
        missionList.Sort((a, b) => { return b.SortOrderRef.CompareTo(a.SortOrderRef); });
        foreach (MissionsUI ui in missionList)
        {
            switch ((MISSION_TYPE)ui.Mission.MissionType)
            {
                case MISSION_TYPE.DAILY:
                    ui.gameObject.transform.SetParent(DailyMissionsRect);
                    break;
                case MISSION_TYPE.WEEKLY:
                    ui.gameObject.transform.SetParent(WeeklyMissionsRect);
                    break;
                case MISSION_TYPE.SEASONAL:
                    ui.gameObject.transform.SetParent(SeasonalMissionsRect);
                    break;
            }
            ui.gameObject.transform.localScale = Vector3.one;

        }

    }

    private void ClaimCompletedMission(MissionsUI mission)
    {
        MissionManager.Instance.ClaimMission(mission.Mission);
        CurrencyManager.Instance.UpdateTokenAmount(mission.Mission.TokensEarned);
        PopUpManager.Instance.AddCurrencyPopUp(CURRENCY_TYPE.TOKEN, mission.Mission.TokensEarned, mission.ButtonPosition);
        mission.SetCompleted();
    }

    private void OnPlayerStatsUpdated()
    {
        UpdateMissionsRect();
    }

}
