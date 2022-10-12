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
        Debug.LogWarning("Could not find Avatar Cosmetic of ID " + id + "!");
        return null;
    }

    public Sprite GetDisplaySprite(int cosmeticID, BODY_PART_TYPE partType)
    {
        foreach (DisplayCosmeticSO discos in CosmeticDisplaySpritesList)
        {
            if (discos.CosmeticID == cosmeticID && discos.partType == partType)
                return discos.DisplaySprite;
        }

        Debug.LogWarning("Could not Find Cosmetic Display Sprite for cosmetic of ID " + cosmeticID);
        return null;

    }

  

}


