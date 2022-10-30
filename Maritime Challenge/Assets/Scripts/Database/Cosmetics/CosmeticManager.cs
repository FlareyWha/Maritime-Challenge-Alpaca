using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CosmeticManager : MonoBehaviourSingleton<CosmeticManager>
{
    [SerializeField]
    private List<CosmeticSO> avatarCosmeticsList;
    [SerializeField]
    private List<DisplayCosmeticSO> CosmeticDisplaySpritesList;

    protected override void Awake()
    {
        base.Awake();

        foreach (KeyValuePair<Cosmetic, bool> cosmetic in PlayerData.CosmeticsList)
        {
            cosmetic.Key.LinkedCosmetic = FindCosmeticByID(cosmetic.Key.CosmeticID);
        }
    }

    public CosmeticSO FindCosmeticByID(int id)
    {
        foreach (CosmeticSO cos in avatarCosmeticsList)
        {
            if (cos.ID == id)
                return cos;
        }
        // Debug.LogWarning("Could not find Avatar Cosmetic of ID " + id + "!");
        return null;
    }

    public Sprite GetDisplaySprite(int cosmeticID, BODY_PART_TYPE partType)
    {
        if (cosmeticID == 3 && partType == BODY_PART_TYPE.HAIR_BACK)
            return null;

        foreach (DisplayCosmeticSO discos in CosmeticDisplaySpritesList)
        {
            if (discos.CosmeticID == cosmeticID && discos.partType == partType)
                return discos.DisplaySprite;
        }

        Debug.LogWarning("Could not Find Cosmetic Display Sprite for cosmetic of ID " + cosmeticID);
        return null;
    }

    public void BuyCosmetic(int cosmeticID)
    {
        StartCoroutine(DoBuyCosmetic(cosmeticID));
    }

    IEnumerator DoBuyCosmetic(int cosmeticID)
    {
        string url = ServerDataManager.URL_updateCosmeticList;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        form.AddField("iCosmeticID", cosmeticID);
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

    public void UpdateEquippedCosmetic(int oldID, int newID)
    {
        StartCoroutine(DoUpdateEquippedCosmeticList(newID, oldID));
    }

    IEnumerator DoUpdateEquippedCosmeticList(int newCosmeticID, int oldCosmeticID)
    {
        string url = ServerDataManager.URL_updateEquippedCosmeticList;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", PlayerData.UID);
        form.AddField("iNewCosmeticID", newCosmeticID);
        form.AddField("iOldCosmeticID", oldCosmeticID);
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

    public static COSMETIC_TYPE BodyPartToCosmetic(BODY_PART_TYPE type)
    {
        switch (type)
        {
            case BODY_PART_TYPE.HAIR_BACK:
            case BODY_PART_TYPE.HAIR_FRONT:
                return COSMETIC_TYPE.HAIR;
            default:
                return (COSMETIC_TYPE)(int)type;
        }
    }

}


