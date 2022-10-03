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

    public delegate void AvatarSaved();
    public static event AvatarSaved OnAvatarUpdated;

    void Start()
    {
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

        foreach (KeyValuePair<Cosmetic, bool> cos in GameSettings.CosmeticsList)
        {
            AvatarItemUI item = Instantiate(AvatarItemUIPrefab, CustomisablesRect[(int)cos.Key.cosmeticBodyPartType]).GetComponent<AvatarItemUI>();
            item.Init(cos.Key.LinkedCosmetic, EquipAccessory);
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

        foreach (KeyValuePair<Cosmetic, bool> cos in GameSettings.CosmeticsList)
        {
            if (cos.Key.cosmeticBodyPartType != type)
                return;

            AvatarItemUI item = Instantiate(AvatarItemUIPrefab, rect).GetComponent<AvatarItemUI>();
            item.Init(cos.Key.LinkedCosmetic, EquipAccessory);
        }
    }

    private void EquipAccessory(AvatarCosmetic part)
    {
        MyAvatar.avatarParts[(int)part.bodyPartType].cosmetic = part;
        OnAvatarUpdated?.Invoke();
    }
}
