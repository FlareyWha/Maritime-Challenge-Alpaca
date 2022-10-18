using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MissionManager : MonoBehaviour
{
    [SerializeField]
    private List<MissionSO> MissionDataList;

    void Awake()
    {
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
    // Start is called before the first frame update
    void Start()
    {
        PlayerData.OnPlayerStatsUpdated += CheckMissionUnlocked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckMissionUnlocked()
    {
        //Idk tbh
    }

    public void ClaimMission(Mission mission)
    {
        Debug.Log("Mission Manager: Claiming Mission.." + mission.MissionName);
        StartCoroutine(DoClaimMission(mission));
    }

    IEnumerator DoClaimMission(Mission mission)
    {
        string url = ServerDataManager.URL_updateAchievementClaimed;
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
}
