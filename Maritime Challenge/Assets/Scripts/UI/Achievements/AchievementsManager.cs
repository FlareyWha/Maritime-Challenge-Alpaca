using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class AchievementsManager : MonoBehaviour
{
    [SerializeField]
    private List<AchievementSO> AchievementDataList;


    [SerializeField]
    private Transform AchievementsRect;
    [SerializeField]
    private GameObject AchievementsUIPrefab;

    private AchievementStatus[] achievements = new AchievementStatus[(int)AchievementType.NumTotal];

    void Awake()
    {
        foreach (KeyValuePair<Achievement, bool> achvment in PlayerData.AchievementList)
        {
            achvment.Key.AchievementData = FindTitleByID(achvment.Key.AchievementID);
        }
    }
    private AchievementSO FindTitleByID(int id)
    {
        foreach (AchievementSO achvment in AchievementDataList)
        {
            if (achvment.ID == id)
                return achvment;
        }
        Debug.LogWarning("Could not find Achievement of ID " + id + "!");
        return null;
    }
    private void Start()
    {
        SetAchievements();

        InitAchievementsRect();
    }

    private void SetAchievements()
    {
        foreach (KeyValuePair<Achievement, bool> achievement in PlayerData.AchievementList)
        {
            AchievementSO data = achievement.Key.AchievementData;
            achievements[(int)data.Type].achievementsList[data.Tier] = achievement.Key;
            if (achievement.Value)
                achievements[(int)data.Type].CheckCurrentTierByUnlocked(data.Tier);
        }
    }
    private void InitAchievementsRect()
    {
        foreach (AchievementStatus achievementStat in achievements)
        {
            Achievement achievement = achievementStat.GetCurrentAchievement();
            AchievementsUI ui = Instantiate(AchievementsUIPrefab, AchievementsRect).GetComponent<AchievementsUI>();
            ui.Init(achievement, GetCurrentProgressNum(achievement.AchievementData.Type), achievement.AchievementData.RequirementNum, ClaimAchievement);
        }
    }

    private int GetCurrentProgressNum(AchievementType type)
    {
        switch (type)
        {
            case AchievementType.Friends:
                return PlayerStats.FriendsAdded;
            case AchievementType.RightShipeida:
                return PlayerStats.RightshipediaEntriesUnlocked;
            case AchievementType.BattleshipsOwned:
                return PlayerStats.BattleshipsOwned;
            default:
                return 0;
        }
    }
    public void CheckAchievementUnlocked(int achievementID, int progressNumber)
    {
        foreach (KeyValuePair<Achievement, bool> achievement in PlayerData.AchievementList)
        {
            if (achievement.Key.AchievementID == achievementID)
            {
                UpdateAchievementUI(progressNumber >= achievement.Key.AchievementData.RequirementNum);
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

public class AchievementStatus
{
    public Achievement[] achievementsList;
    private int currTier = 1;

    private int MaxTier = 3;


    public void CheckCurrentTierByUnlocked(int unlocked_tier)
    {
        if (unlocked_tier + 1 > currTier)
        {
            currTier = unlocked_tier + 1;
            if (currTier > MaxTier)
                currTier = MaxTier;
        }
    }

    public Achievement GetCurrentAchievement()
    {
        return achievementsList[currTier - 1];
    }
}
