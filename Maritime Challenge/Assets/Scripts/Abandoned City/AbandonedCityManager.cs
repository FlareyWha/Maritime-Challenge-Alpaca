using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;

public class AbandonedCityManager : MonoBehaviourSingleton<AbandonedCityManager>
{
    private List<BaseAbandonedCity> abandonedCities = new List<BaseAbandonedCity>();

    private List<JSONAbandonedCity> abandonedCityInfo = new List<JSONAbandonedCity>();

    private GameObject abandonedCityPrefab;

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

    void CreateAbandonedCities()
    {
        //Get the abandoned cities from database???
        //abandonedCities.Clear();
        //abandonedCities.AddRange(FindObjectsOfType<BaseAbandonedCity>());
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
    
    [Server]
    void SpawnAbandonedCities()
    {
        for (int i = 0; i < abandonedCityInfo.Count; ++i)
        {
            BaseAbandonedCity abandonedCity = Instantiate(abandonedCityPrefab, new Vector3(abandonedCityInfo[i].fAbandonedCityXPos, abandonedCityInfo[i].fAbandonedCityYPos, 0), Quaternion.identity).GetComponent<BaseAbandonedCity>();
            NetworkServer.Spawn(abandonedCity.gameObject);
            abandonedCity.InitAbandonedCity(abandonedCityInfo[i].iAbandonedCityID, abandonedCityInfo[i].iAbandonedCityAreaCellWidth, abandonedCityInfo[i].iAbandonedCityAreaCellHeight, new Vector2(abandonedCityInfo[i].fAbandonedCityXPos, abandonedCityInfo[i].fAbandonedCityYPos), abandonedCityInfo[i].iCapturedGuildID);
        }
    }
}
