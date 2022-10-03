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
    [SerializeField]
    private Transform[] CustomisablesRect;



     
    private void UpdateInventoryRect(Transform inventoryRect)
    {
        foreach (Transform child in inventoryRect)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<int, bool> cos in GameSettings.CosmeticsStatusDict)
        {

        }
    }

    private void EquipAccessory(AvatarCosmetic part)
    {
        MyAvatar.avatarParts[(int)part.bodyPartType].cosmetic = part;
    }
}
