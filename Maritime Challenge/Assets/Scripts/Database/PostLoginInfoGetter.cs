using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PostLoginInfoGetter : MonoBehaviour
{
    private LoginManager loginManager;

    private static bool bGetCosmeticsComplete = false;

    private void Start()
    {
        loginManager = GetComponent<LoginManager>();
    }

    public IEnumerator GetInfo()
    {
        CoroutineCollection coroutineCollectionManager = new CoroutineCollection();

        StartCoroutine(coroutineCollectionManager.CollectCoroutine(DoGetCosmetics()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(GetPlayerData()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(GetPlayerStats()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(GetFriends()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(FriendRequestHandler.GetSentFriendRequests()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(FriendRequestHandler.GetRecievedFriendRequests()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(DoGetTitles()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(DoGetAchievements()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(DoGetMissions()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(DoGetBattleships()));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(DoGetMail()));

        //Wait for all the coroutines to finish running before continuing
        yield return coroutineCollectionManager;

        PlayerStatsManager.Instance.SaveAllStats();

        StartCoroutine(coroutineCollectionManager.CollectCoroutine(UpdateLastLoginTime()));

        StartCoroutine(coroutineCollectionManager.CollectCoroutine(GetPhonebookData()));

        yield return coroutineCollectionManager;

        //Connect to server once all the info has been recieved
        loginManager.ConnectToServer();
    }

    IEnumerator GetPlayerData()
    {
        string url = ServerDataManager.URL_getPlayerData;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("Get player data success");

                //Deseralize the data
                JSONDeseralizer.DeseralizePlayerData(webreq.downloadHandler.text);


                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator GetPlayerStats()
    {
        string url = ServerDataManager.URL_getPlayerStats;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                JSONDeseralizer.DeseralizePlayerStats(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }
    IEnumerator GetFriends()
    {
        string url = ServerDataManager.URL_getFriends;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("Get friends success");

                //Deseralize the data
                JSONDeseralizer.DeseralizeFriends(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator GetPhonebookData()
    {
        string url = ServerDataManager.URL_getPhonebookData;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                JSONDeseralizer.DeseralizePhonebookData(webreq.downloadHandler.text);

                foreach (KeyValuePair<int, BasicInfo> other in PlayerData.PhonebookData)
                {
                    StartCoroutine(GetEquippedCosmetics(other.Key, false));
                }
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator DoGetCosmetics()
    {
        string url = ServerDataManager.URL_getCosmeticData;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                PlayerData.CosmeticsList = JSONDeseralizer.DeseralizeCosmeticData(webreq.downloadHandler.text);
                bGetCosmeticsComplete = true;

                yield return StartCoroutine(GetEquippedCosmetics(PlayerData.UID, true));
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator GetEquippedCosmetics(int uid, bool self)
    {
        string url = ServerDataManager.URL_getEquippedCosmeticList;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", uid);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:

                while (!bGetCosmeticsComplete)
                    yield return null;
             
                List<int> equippedCosmeticListIDList = JSONDeseralizer.DeseralizeEquippedCosmeticList(webreq.downloadHandler.text);

                List<Cosmetic> cosmeticList = new List<Cosmetic>();

                for (int i = 0; i < equippedCosmeticListIDList.Count; ++i)
                {
                    foreach (KeyValuePair<Cosmetic, bool> cosmetic in PlayerData.CosmeticsList)
                    {
                        if (cosmetic.Key.CosmeticID == equippedCosmeticListIDList[i])
                        {
                            if (self)
                                PlayerData.EquippedCosmeticsList.Add(cosmetic.Key);
                            else
                                cosmeticList.Add(cosmetic.Key);

                            break;
                        }
                    }
                }

                if (!self)
                    PlayerData.OthersEquippedCosmeticList.Add(uid, cosmeticList);

                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator DoGetTitles()
    {
        string url = ServerDataManager.URL_getTitleData;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                PlayerData.TitleDictionary = JSONDeseralizer.DeseralizeTitleData(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator DoGetAchievements()
    {
        string url = ServerDataManager.URL_getAchievementData;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                PlayerData.AchievementList = JSONDeseralizer.DeseralizeAchievementData(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator DoGetMissions()
    {
        string url = ServerDataManager.URL_getMissionData;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                PlayerData.MissionList = JSONDeseralizer.DeseralizeMissionData(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator DoGetBattleships()
    {
        string url = ServerDataManager.URL_getBattleshipData;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                PlayerData.BattleshipList = JSONDeseralizer.DeseralizeBattleshipData(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

  

    IEnumerator DoGetMail()
    {
        string url = ServerDataManager.URL_getMailData;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                PlayerData.MailList.Clear();
                PlayerData.MailList.AddRange(JSONDeseralizer.DeseralizeMailData(webreq.downloadHandler.text));
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server Error");
                break;
        }
    }

    IEnumerator UpdateLastLoginTime()
    {
        // Increase Login - if daily reset, increase login in func
        if (!PlayerData.CheckForDailyReset())
            PlayerStatsManager.Instance.UpdatePlayerStat(PLAYER_STAT.LOGIN, ++PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.LOGIN]);

        //Set the URL to the getUID one
        string url = ServerDataManager.URL_updateLastLoginTime;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
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
                Debug.LogError(webreq.downloadHandler.text);
                break;
        }
    }
}
