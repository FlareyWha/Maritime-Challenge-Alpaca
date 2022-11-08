using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarCustomisationManager : MonoBehaviourSingleton<AvatarCustomisationManager>
{
    [SerializeField]
    private GameObject AvatarItemUIPrefab;
    [SerializeField]
    private Transform[] CustomisablesRect;

    [SerializeField]
    private AvatarDisplay displayAvatar;

    private AvatarItemUI[] currentEquippedItem = new AvatarItemUI[(int)COSMETIC_TYPE.NUM_TOTAL];
    private PlayerAvatarManager playerAvatarManager = null;

    void Start()
    {
        StartCoroutine(Inits());
    }

    IEnumerator Inits()
    {
        while (PlayerData.MyPlayer == null)
            yield return null;

        playerAvatarManager = PlayerData.MyPlayer.gameObject.GetComponent<PlayerAvatarManager>();

        while (!playerAvatarManager.IsInitted())
            yield return null;
        displayAvatar.SetPlayer(PlayerData.MyPlayer);
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
            // if not unlocked
            if (!cos.Value)
                continue;

            AvatarItemUI item = Instantiate(AvatarItemUIPrefab, CustomisablesRect[(int)cos.Key.CosmeticBodyPartType]).GetComponent<AvatarItemUI>();
            item.Init(cos.Key, EquipAccessory);

            // If Equipped
            if (playerAvatarManager.GetEquippedCosmetic(cos.Key.CosmeticBodyPartType) == cos.Key.LinkedCosmetic)
            {
                currentEquippedItem[(int)cos.Key.CosmeticBodyPartType] = item;
                item.SetEquippedOverlay(true);
            }
        }
    }
     
    public void UpdateInventoryRect(COSMETIC_TYPE type)
    {
        // Clear
        Transform rect = CustomisablesRect[(int)type];
        foreach (Transform child in rect)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<Cosmetic, bool> cos in PlayerData.CosmeticsList)
        {
            Debug.Log("Checking for " + cos.Key.CosmeticName);
            if (cos.Key.CosmeticBodyPartType != type || !cos.Value)
                continue;

            AvatarItemUI item = Instantiate(AvatarItemUIPrefab, rect).GetComponent<AvatarItemUI>();
            item.Init(cos.Key, EquipAccessory);
        }
    }

    private void EquipAccessory(AvatarItemUI item)
    {
        // If is current item, return
        if (currentEquippedItem[(int)item.Cosmetic.CosmeticBodyPartType] == item)
            return;

        // set select overlay to false for prev item
        if (currentEquippedItem[(int)item.Cosmetic.CosmeticBodyPartType] != null)
            currentEquippedItem[(int)item.Cosmetic.CosmeticBodyPartType].SetEquippedOverlay(false);
        currentEquippedItem[(int)item.Cosmetic.CosmeticBodyPartType] = item;

        // Update My Avatar
        UpdateAvatar(item.Cosmetic);
    }

    private void UpdateAvatar(Cosmetic cos)
    {
        // Database
        CosmeticManager.Instance.UpdateEquippedCosmetic(playerAvatarManager.GetEquippedCosmeticID(cos.CosmeticBodyPartType), cos.CosmeticID);
      
        playerAvatarManager.AvatarCosmeticChanged(cos.CosmeticBodyPartType, cos.CosmeticID);

        
    }
}
