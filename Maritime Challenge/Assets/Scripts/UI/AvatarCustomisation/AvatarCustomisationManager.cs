using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarCustomisationManager : MonoBehaviourSingleton<AvatarCustomisationManager>
{
    [SerializeField]
    private AvatarSO MyAvatar;
    [SerializeField]
    private GameObject AvatarItemUIPrefab;
    [SerializeField]
    private Transform[] CustomisablesRect;

    [SerializeField]
    private AvatarDisplay displayAvatar;


    private AvatarItemUI[] currentEquippedItem = new AvatarItemUI[(int)COSMETIC_TYPE.NUM_TOTAL];

    void Start()
    {
        StartCoroutine(Inits());
    }

    IEnumerator Inits()
    {
        while (PlayerData.MyPlayer == null)
            yield return null;

        //displayAvatar.set
        UpdateAllInventoryRects();
    

    }

    private void UpdateAllInventoryRects()
    {
        // Clear
        foreach (Transform rectTransform in CustomisablesRect)
        {
            foreach (Transform child in rectTransform)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (KeyValuePair<Cosmetic, bool> cos in PlayerData.CosmeticsList)
        {
            AvatarItemUI item = Instantiate(AvatarItemUIPrefab, CustomisablesRect[(int)cos.Key.CosmeticBodyPartType]).GetComponent<AvatarItemUI>();
            item.Init(cos.Key, EquipAccessory);

            // If Equipped
            if (MyAvatar.avatarParts[(int)cos.Key.CosmeticBodyPartType].cosmetic == cos.Key.LinkedCosmetic)
            {
                currentEquippedItem[(int)cos.Key.CosmeticBodyPartType] = item;
                item.SetEquippedOverlay(true);
            }
        }
    }
     
    private void UpdateInventoryRect(COSMETIC_TYPE type)
    {
        // Clear
        Transform rect = CustomisablesRect[(int)type];
        foreach (Transform child in rect)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<Cosmetic, bool> cos in PlayerData.CosmeticsList)
        {
            if (cos.Key.CosmeticBodyPartType != type)
                return;

            AvatarItemUI item = Instantiate(AvatarItemUIPrefab, rect).GetComponent<AvatarItemUI>();
            item.Init(cos.Key, EquipAccessory);
        }
    }

    private void EquipAccessory(AvatarItemUI item)
    {
        if (currentEquippedItem[(int)item.Cosmetic.CosmeticBodyPartType] != null)
            currentEquippedItem[(int)item.Cosmetic.CosmeticBodyPartType].SetEquippedOverlay(false);
        currentEquippedItem[(int)item.Cosmetic.CosmeticBodyPartType] = item;


        // Update My Avatar
        UpdateAvatar(item.Cosmetic);
    }

    private void UpdateAvatar(Cosmetic cos)
    {

        switch (cos.CosmeticBodyPartType)
        {
            case COSMETIC_TYPE.HAIR:
                MyAvatar.avatarParts[(int)BODY_PART_TYPE.HAIR_FRONT].cosmetic = cos.LinkedCosmetic;
                PlayerData.CommandsHandler.SendAvatarChanged(BODY_PART_TYPE.HAIR_FRONT, cos.CosmeticBodyPartID);
                MyAvatar.avatarParts[(int)BODY_PART_TYPE.HAIR_BACK].cosmetic = cos.LinkedCosmetic;
                PlayerData.CommandsHandler.SendAvatarChanged(BODY_PART_TYPE.HAIR_BACK, cos.CosmeticBodyPartID);
                break;
            default:
                MyAvatar.avatarParts[(int)cos.CosmeticBodyPartType].cosmetic = cos.LinkedCosmetic;
                PlayerData.CommandsHandler.SendAvatarChanged((BODY_PART_TYPE)cos.CosmeticBodyPartType, cos.CosmeticBodyPartID);
                break;
        }
        
    }
}
