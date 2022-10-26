using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MissionManager : MonoBehaviourSingleton<MissionManager>
{
    [SerializeField]
    private List<MissionSO> MissionDataList;

    protected override void Awake()
    {
        base.Awake();
        // Link Mission Scriptable Object to Mission Class from database
        foreach (KeyValuePair<Mission, bool> mission in PlayerData.MissionList)
        {
            mission.Key.MissionData = FindMissionByID(mission.Key.MissionID);
        }
    }
    private MissionSO FindMissionByID(int id)
    {
        foreach (MissionSO mission in MissionDataList)
        {
            if (mission.ID == id)
                return mission;
        }
        Debug.LogWarning("Could not find Mission of ID " + id + "!");
        return null;
    }
   


    public void ClaimMission(Mission mission)
    {
        Debug.Log("Mission Manager: Claiming Mission.." + mission.MissionName);
        StartCoroutine(DoClaimMission(mission));
    }

    IEnumerator DoClaimMission(Mission mission)
    {
        string url = ServerDataManager.URL_updateMissionClaimed;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        form.AddField("iMissionID", mission.MissionID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                PlayerData.SetMissionClaimed(mission);
                Debug.Log(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    public void ResetMissions(MISSION_TYPE missionType)
    {
        StartCoroutine(DoResetMissions(missionType));
    }

    IEnumerator DoResetMissions(MISSION_TYPE missionType)
    {
        string url = ServerDataManager.URL_resetMissions;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iMissionType", (int)missionType);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }
}
