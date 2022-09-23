using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;

public class AbandonedCityManager : MonoBehaviourSingleton<AbandonedCityManager>
{
    private List<JSONAbandonedCity> abandonedCityInfo = new List<JSONAbandonedCity>();

    [SerializeField]
    private GameObject abandonedCityPrefab;

    [SerializeField]
    private Transform abandonedCityContainer;

    // Start is called before the first frame update
    void Start()
    {
        //Only server should call this

        CreateAbandonedCities();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Server]
    void CreateAbandonedCities()
    {
        //Get the abandoned cities from database
        StartCoroutine(GetAbandonedCityInfo());
    }

    IEnumerator GetAbandonedCityInfo()
    {
        string url = ServerDataManager.URL_getAbandonedCityInfo;
        Debug.Log(url);

        using UnityWebRequest webreq = UnityWebRequest.Get(url);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize and instantiate somehow idk tbh
                abandonedCityInfo = JSONDeseralizer.DeseralizeAbandonedCityInfo(webreq.downloadHandler.text);
                SpawnAbandonedCities();
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError(webreq.downloadHandler.text);
                break;
        }
    }
    
    void SpawnAbandonedCities()
    {
        for (int i = 0; i < abandonedCityInfo.Count; ++i)
        {
            BaseAbandonedCity abandonedCity = Instantiate(abandonedCityPrefab, new Vector3(abandonedCityInfo[i].fAbandonedCityXPos, abandonedCityInfo[i].fAbandonedCityYPos, 0), Quaternion.identity, abandonedCityContainer).GetComponent<BaseAbandonedCity>();
            NetworkServer.Spawn(abandonedCity.gameObject);

            abandonedCity.InitAbandonedCity(abandonedCityInfo[i].iAbandonedCityID, abandonedCityInfo[i].iAbandonedCityAreaCellWidth, abandonedCityInfo[i].iAbandonedCityAreaCellHeight, new Vector2(abandonedCityInfo[i].fAbandonedCityXPos, abandonedCityInfo[i].fAbandonedCityYPos), abandonedCityInfo[i].iCapturedGuildID);

            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(abandonedCity.gameObject, UnityEngine.SceneManagement.SceneManager.GetSceneByName(PlayerData.activeSubScene));
        }
    }
}
