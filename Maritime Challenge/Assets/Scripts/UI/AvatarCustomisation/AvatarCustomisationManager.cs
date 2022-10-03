using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private AvatarSO MyAvatar;
    [SerializeField]
    private GameObject AvatarItemUIPrefab;



     
    private void UpdateInventoryRect()
    {

    }

    private void EquipAccessory(AvatarCosmetic part)
    {
        MyAvatar.avatarParts[(int)part.bodyPartType].cosmetic = part;
    }
}
