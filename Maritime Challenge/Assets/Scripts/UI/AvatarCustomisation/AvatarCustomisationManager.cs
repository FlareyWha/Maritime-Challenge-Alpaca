using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private AvatarSO MyAvatar;
    [SerializeField]
    private GameObject AvatarItemUIPrefab;
   // [SerializeField]
   // private Transform 



     
    private void UpdateInventoryRect()
    {

    }

    private void EquipAccessory(AvatarCosmetic part)
    {
        MyAvatar.avatarParts[(int)part.bodyPartType].cosmetic = part;
    }
}
