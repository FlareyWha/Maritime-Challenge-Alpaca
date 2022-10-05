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

    private PlayerAnimationsManager myPlayerAnimationsManager;

    private AvatarItemUI[] currentEquippedItem = new AvatarItemUI[(int)CosmeticType.NUM_TOTAL];

    void Start()
    {
        StartCoroutine(Inits());
    }

    IEnumerator Inits()
    {
        while (PlayerData.MyPlayer == null)
            yield return null;
        myPlayerAnimationsManager = PlayerData.MyPlayer.gameObject.GetComponent<PlayerAnimationsManager>();
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
            item.Init(cos.Key.LinkedCosmetic, EquipAccessory);

            // If Equipped
            if (MyAvatar.avatarParts[(int)cos.Key.CosmeticBodyPartType].cosmetic == cos.Key.LinkedCosmetic)
            {
                currentEquippedItem[(int)cos.Key.CosmeticBodyPartType] = item;
                item.SetEquippedOverlay(true);
            }
        }
    }
     
    private void UpdateInventoryRect(CosmeticType type)
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
            item.Init(cos.Key.LinkedCosmetic, EquipAccessory);
        }
    }

    private void EquipAccessory(AvatarItemUI item)
    {
        if (currentEquippedItem[(int)item.Cosmetic.bodyPartType] != null)
            currentEquippedItem[(int)item.Cosmetic.bodyPartType].SetEquippedOverlay(false);
        currentEquippedItem[(int)item.Cosmetic.bodyPartType] = item;
        MyAvatar.avatarParts[(int)item.Cosmetic.bodyPartType].cosmetic = item.Cosmetic;

        PlayerData.CommandsHandler.SendAvatarChanged(item.Cosmetic.bodyPartType, item.Cosmetic.ID, MyAvatar.avatarParts[(int)item.Cosmetic.bodyPartType].bodyPartName);
    }
}
