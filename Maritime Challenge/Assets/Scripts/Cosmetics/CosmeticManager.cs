using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CosmeticManager : MonoBehaviour
{
    [SerializeField]
    private List<AvatarCosmetic> avatarCosmeticsList;

    void Awake()
    {
        foreach (KeyValuePair<Cosmetic, bool> cosmetic in GameSettings.CosmeticsList)
        {
            cosmetic.Key.LinkedCosmetic = FindCosmeticByID(cosmetic.Key.cosmeticID);
        }
    }

    private AvatarCosmetic FindCosmeticByID(int id)
    {
        foreach (AvatarCosmetic cos in avatarCosmeticsList)
        {
            if (cos.ID == id)
                return cos;
        }
        Debug.LogWarning("Could not find Avatar Cosmetic of ID " + id + "!");
        return null;
    }

}


