using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BattleshipShopManager : MonoBehaviour
{
    [SerializeField]
    private GameObject battleshipShopItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyItem(int battleshipID, int price)
    {
        if (PlayerData.NumTokens >= price)
        {
            UpdateTokenAmount(price);
            UnlockBattleship(battleshipID);
            UpdateUI();
            //Confirmation text to say purchase successful
        }
        else
        {
            //Confirmation text to say purchase not successful
        }
    }

    void UpdateTokenAmount(int price)
    {
        PlayerData.NumTokens -= price;
        StartCoroutine(CurrencyManager.DoUpdateTokenAmount());

        //Update the UI of the total tokens if needed
    }

    void UnlockBattleship(int battleshipID)
    {
        StartCoroutine(DoUnlockBattleship(battleshipID));

        //For each loop to update the local list, but idk how to do the local list cus battleship is this one huge class
    }

    IEnumerator DoUnlockBattleship(int battleshipID)
    {
        string url = ServerDataManager.URL_updateBattleshipList;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        form.AddField("iBattleshipID", battleshipID);
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

    void UpdateUI()
    {
        //Update UI to show that battleship has been bought alr
    }
}
