using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MissionManager : MonoBehaviour
{
    [SerializeField]
    private List<MissionSO> MissionDataList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
