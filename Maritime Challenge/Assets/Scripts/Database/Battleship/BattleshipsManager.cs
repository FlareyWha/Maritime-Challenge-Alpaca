using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BattleshipsManager : MonoBehaviourSingleton<BattleshipsManager>
{
    [SerializeField]
    private List<BattleshipSO> battleshipsList;

    protected override void Awake()
    {
        base.Awake();

        foreach (KeyValuePair<BattleshipInfo, bool> ship in PlayerData.BattleshipList)
        {
            ship.Key.BattleshipData = FindBattleshipByID(ship.Key.BattleshipID);
        }
    }

    private BattleshipSO FindBattleshipByID(int id)
    {
        foreach (BattleshipSO ship in battleshipsList)
        {
            if (ship.ID == id)
                return ship;
        }
        Debug.LogWarning("Could not find Battleship of ID " + id + "!");
        return null;
    }

    public void UnlockBattleship(int battleshipID)
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

    public void UpdateCurrentBattleship(int battleshipID)
    {
        PlayerData.CurrentBattleship = battleshipID;
        StartCoroutine(DoUpdateCurrentBattleship(battleshipID));
    }

    IEnumerator DoUpdateCurrentBattleship(int battleshipID)
    {
        string url = ServerDataManager.URL_updateCurrentBattleship;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("iCurrentBattleship", battleshipID);
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

   
}

public class BattleshipInfo
{
    public int BattleshipID;
    public string BattleshipName;
    public int HP;
    public int Atk;
    public float AtkSpd;
    public float CritRate;
    public float CritDmg;
    public float MoveSpd;

    public BattleshipSO BattleshipData;

    public BattleshipInfo(int ID, string name, int hp, int atk, float atkSpd, float critRate, float critDmg, float moveSpd)
    {
        BattleshipID = ID;
        BattleshipName = name;
        HP = hp;
        Atk = atk;
        AtkSpd = atkSpd;
        CritRate = critRate;
        CritDmg = critDmg;
        MoveSpd = moveSpd;
    }
}