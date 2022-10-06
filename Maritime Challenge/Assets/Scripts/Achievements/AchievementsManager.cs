using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class AchievementsManager : MonoBehaviour
{
    [SerializeField]
    private Transform AchievementsRect;
    [SerializeField]
    private GameObject AchievementsUIPrefab;


    private void Start()
    {
        InitAchievementsRect();
    }
    private void InitAchievementsRect()
    {
        foreach (KeyValuePair<Achievement, bool> achievement in PlayerData.AchievementList)
        {
            AchievementsUI ui = Instantiate(AchievementsUIPrefab, AchievementsRect).GetComponent<AchievementsUI>();
            ui.Init(achievement.Key, ClaimAchievement);
        }
    }

    public void CheckAchievementUnlocked(int achievementID, int progressNumber)
    {
        foreach (KeyValuePair<Achievement, bool> achievement in PlayerData.AchievementList)
        {
            if (achievement.Key.AchievementID == achievementID)
            {
                UpdateAchievementUI(progressNumber >= achievement.Key.AchievementRequirementMaxNumber);
            }
        }
    }

    void UpdateAchievementUI(bool unlocked)
    {
        if (unlocked)
        {

        }
        else
        {

        }
    }

    public void ClaimAchievement(Achievement achievement)
    {
        StartCoroutine(DoClaimAchievement(achievement.AchievementID));
        //Achievement achievement = GetAchievement(achievementID);

        PlayerData.AchievementList[achievement] = true;
        int earnedTitleID = achievement.EarnedTitleID;

        if (earnedTitleID != -1)
        {
            foreach (KeyValuePair<Title, bool> title in PlayerData.TitleDictionary)
            {
                if (title.Key.TitleID == earnedTitleID)
                {
                    StartCoroutine(UnlockTitle(achievement.EarnedTitleID));
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
