using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RefreshDatabaseManager : MonoBehaviour
{
    [SerializeField]
    private Text confirmationText;

    [SerializeField]
    private GameObject loadingScreenSpin;

    public void RefreshDatabase()
    {
        loadingScreenSpin.SetActive(true);
        confirmationText.gameObject.SetActive(false);
        StartCoroutine(DoRefreshDatabase());
    }

    public IEnumerator DoRefreshDatabase()
    {
        CoroutineCollection coroutineCollectionManager = new CoroutineCollection();

        StartCoroutine(coroutineCollectionManager.CollectCoroutine(AddData(ServerDataManager.URL_addAchievementListData)));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(AddData(ServerDataManager.URL_addMissionListData)));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(AddData(ServerDataManager.URL_addBattleshipListData)));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(AddData(ServerDataManager.URL_addCosmeticListData)));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(AddData(ServerDataManager.URL_addTitleListData)));

        //Wait for all the coroutines to finish running before continuing
        yield return coroutineCollectionManager;

        confirmationText.text = "Database refreshed";
        loadingScreenSpin.SetActive(false);
        confirmationText.gameObject.SetActive(true);
    }

    IEnumerator AddData(string url)
    {
        Debug.Log(url);

        using UnityWebRequest webreq = UnityWebRequest.Get(url);
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
