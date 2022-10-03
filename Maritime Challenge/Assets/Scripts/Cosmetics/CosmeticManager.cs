using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CosmeticManager : MonoBehaviour
{
    [SerializeField]
    private List<AvatarCosmetic> avatarCosmeticsList;
   
   

    public void GetCosmetics()
    {
        StartCoroutine(DoGetCosmetics());
        StartCoroutine(DoGetCosmeticStatusList());
    }

   
}


