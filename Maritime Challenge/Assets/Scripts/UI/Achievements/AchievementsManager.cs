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
        // Init AchievementStatus Array
        for (int i = 0; i < achievements.Length; i++)
        {
            achievements[i] = new AchievementStatus();
        }

        // Link Achievement Scriptable Object to Achievement Class from database
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
        PlayerData.OnPlayerStatsUpdated += CheckAchievementUnlocked;

        SetAchievementsStatus();
        UpdateAchievementsRect();
    }

    private void SetAchievementsStatus()
    {
        foreach (KeyValuePair<Achievement, bool> achievement in PlayerData.AchievementList)
        {
            AchievementSO data = achievement.Key.AchievementData;
            achievements[(int)data.Type].achievementsList[data.Tier - 1] = achievement.Key;
            if (achievement.Value)
                achievements[(int)data.Type].CheckCurrentTierByUnlocked(data.Tier);
        }
    }
    private void UpdateAchievementsRect()
    {
        // Clear Rect
        foreach (Transform child in AchievementsRect)
        {
            Destroy(child.gameObject);
        }

        // Init Rect
        foreach (AchievementStatus achievementStat in achievements)
        {
            Achievement achievement = achievementStat.GetCurrentAchievement();
            AchievementsUI ui = Instantiate(AchievementsUIPrefab, AchievementsRect).GetComponent<AchievementsUI>();
            int currProgress = GetCurrentProgressNum(achievement.AchievementData.Type);
            int reqProgress = achievement.AchievementData.RequirementNum;
            if (achievementStat.OnFinalTier() && currProgress >= reqProgress)
                ui.SetCompleted(achievement);
            else
                ui.Init(achievement, currProgress , reqProgress, ClaimAchievement);
        }
    }

    private int GetCurrentProgressNum(AchievementType type)
    {
        switch (type)
        {
            case AchievementType.Friends:
                return PlayerData.PlayerStats.FriendsAdded;
            case AchievementType.RightShipeida:
                return PlayerData.PlayerStats.RightshipediaEntriesUnlocked;
            case AchievementType.BattleshipsOwned:
                return PlayerData.PlayerStats.BattleshipsOwned;
            default:
                return 0;
        }
    }

    public void CheckAchievementUnlocked() // Call Whenever PlayerStats is Updated
    {
        //SetAchievementsStatus();
        UpdateAchievementsRect();
    }

   
    public void ClaimAchievement(Achievement achievement)
    {
        Debug.Log("Achievement Manager: Claiming Achievement.." + achievement.AchievementName);
        StartCoroutine(DoClaimAchievement(achievement));
        //Achievement achievement = GetAchievement(achievementID);

        PlayerData.AchievementList[achievement] = true;
        int earnedTitleID = achievement.EarnedTitleID;

        if (earnedTitleID == -1)
        {
            Debug.Log("Title ID was -1! Cannot Earn/Unlock");
            return;
        }

        // Unlock Title
        foreach (KeyValuePair<Title, bool> title in PlayerData.TitleDictionary)
        {
            if (title.Key.TitleID == earnedTitleID)
            {
                StartCoroutine(UnlockTitle(title.Key));
                break;
            }
        }

    }

    private void OnTitleUnlocked(Title title)
    {
        PlayerData.SetTitleUnlocked(title);
        TitlesUIManager.Instance.UpdateTitlesRect();
    }

    IEnumerator DoClaimAchievement(Achievement achvment)
    {
        string url = ServerDataManager.URL_updateAchievementClaimed;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        form.AddField("iAchievementID", achvment.AchievementID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                PlayerData.SetAchievementClaimed(achvment);
                SetAchievementsStatus();
                UpdateAchievementsRect();
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

    IEnumerator UnlockTitle(Title title)
    {
        string url = ServerDataManager.URL_updateTitleList;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        form.AddField("iTitleID", title.TitleID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                OnTitleUnlocked(title);
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
    public Achievement[] achievementsList = new Achievement[MaxTier];
    private int currTier = 1;

    private const int MaxTier = 3;


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

    public bool OnFinalTier()
    {
        return currTier == MaxTier;
    }
}
