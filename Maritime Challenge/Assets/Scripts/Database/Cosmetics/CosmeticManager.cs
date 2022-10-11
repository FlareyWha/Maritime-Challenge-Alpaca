using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CosmeticManager : MonoBehaviour
{
    [SerializeField]
    private List<CosmeticSO> avatarCosmeticsList;

    void Awake()
    {
        foreach (KeyValuePair<Cosmetic, bool> cosmetic in PlayerData.CosmeticsList)
        {
            cosmetic.Key.LinkedCosmetic = FindCosmeticByID(cosmetic.Key.CosmeticID);
        }
    }

    private CosmeticSO FindCosmeticByID(int id)
    {
        foreach (CosmeticSO cos in avatarCosmeticsList)
        {
            if (cos.ID == id)
                return cos;
        }
        Debug.LogWarning("Could not find Avatar Cosmetic of ID " + id + "!");
        return null;
    }

}


