using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public enum ACHIEVEMENT_ID
{
    //Idk what im putting here just some stuff but make sure to put in order unless u wanna make the thing like ur cosmetic

}

public class AchievementsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAchievementUnlocked(ACHIEVEMENT_ID achievementID, int progressNumber)
    {
        foreach (KeyValuePair<Achievement, bool> achievement in PlayerData.AchievementList)
        {
            if (achievement.Key.AchievementID == (int)achievementID)
            {
                SetAchievementUI(progressNumber >= achievement.Key.AchievementRequirementMaxNumber);
            }
        }
    }

    void SetAchievementUI(bool unlocked)
    {
        if (unlocked)
        {

        }
        else
        {

        }
    }

    public void ClaimAchievement(ACHIEVEMENT_ID achievementID)
    {
        StartCoroutine(DoClaimAchievement((int)achievementID));
        Achievement achievement = GetAchievement((int)achievementID);

        PlayerData.AchievementList[achievement] = true;
        int earnedTitleID = achievement.EarnedTitleID;

        if (earnedTitleID != -1)
        {
            foreach (KeyValuePair<Title, bool> title in PlayerData.TitleDictionary)
            {
                if (title.Key.TitleID == earnedTitleID)
                {
                    UnlockTitle(achievement.EarnedTitleID);
                    PlayerData.TitleDictionary[title.Key] = true;
                    break;
                }
            }
        }
    }

    IEnumerator DoClaimAchievement(int achievementID)
    {
        string url = ServerDataManager.URL_updateAchievementClaimed;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        form.AddField("iAchievementID", achievementID);
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

    IEnumerator UnlockTitle(int iTitleID)
    {
        string url = ServerDataManager.URL_updateTitleList;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        form.AddField("iTitleID", iTitleID);
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

    Achievement GetAchievement(int achievementID)
    {
        foreach (KeyValuePair<Achievement, bool> achievement in PlayerData.AchievementList)
        {
            if (achievement.Key.AchievementID == achievementID)
            {
                return achievement.Key;
            }
        }

        return null;
    }
}
