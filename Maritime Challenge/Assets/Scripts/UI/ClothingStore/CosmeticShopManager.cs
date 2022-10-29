using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticShopManager : MonoBehaviour
{
    [SerializeField]
    private GameObject CosmeticShopItemUIPrefab;
    [SerializeField]
    private Text NumTokensText;
    [SerializeField]
    private GameObject[] ShopItemPanels;
    [SerializeField]
    private GameObject[] ShopSelectedTabs;
    [SerializeField]
    private Transform HairItemsRect, HeadwearItemsRect, BodyItemsRect, TopsItemRect, BottomsItemRect, ShoeItemRect;

    [SerializeField]
    private GameObject EmptyPanel;

    [SerializeField]
    private int CurrentlySelectedindex = 0;

    private CosmeticShopItemUI currSelectedShopItem = null;


    private void Awake()
    {
        PlayerData.OnNumTokensUpdated += UpdateNumTokensUI;
        UpdateNumTokensUI();
        UpdateShopTabs();

    }
    private void OnDestroy()
    {
        PlayerData.OnNumTokensUpdated -= UpdateNumTokensUI;
    }

    private void UpdateNumTokensUI()
    {
        NumTokensText.text = PlayerData.NumTokens.ToString();
    }
    private void UpdateShopTabs()
    {
        // Clear Rects
        foreach (Transform child in HairItemsRect)
            Destroy(child.gameObject);
        foreach (Transform child in HeadwearItemsRect)
            Destroy(child.gameObject);
        foreach (Transform child in BodyItemsRect)
            Destroy(child.gameObject);
        foreach (Transform child in TopsItemRect)
            Destroy(child.gameObject);
        foreach (Transform child in BottomsItemRect)
            Destroy(child.gameObject);
        foreach (Transform child in ShoeItemRect)
            Destroy(child.gameObject);

        // Fill Rects
        foreach (KeyValuePair<Cosmetic, bool> cosmeticInfo in PlayerData.CosmeticsList)
        {
            // Skip if Unlocked
            if (cosmeticInfo.Value)
                continue;

            // Rect to instantiate to
            CosmeticShopItemUI itemUI = null;
            switch (cosmeticInfo.Key.CosmeticBodyPartType)
            {
                case COSMETIC_TYPE.HAIR:
                    itemUI = Instantiate(CosmeticShopItemUIPrefab, HairItemsRect).GetComponent<CosmeticShopItemUI>();
                    break;
                case COSMETIC_TYPE.HEADWEAR:
                    itemUI = Instantiate(CosmeticShopItemUIPrefab, HeadwearItemsRect).GetComponent<CosmeticShopItemUI>();
                    break;
                case COSMETIC_TYPE.BODY:
                    itemUI = Instantiate(CosmeticShopItemUIPrefab, BodyItemsRect).GetComponent<CosmeticShopItemUI>();
                    break;
                case COSMETIC_TYPE.TOP:
                    itemUI = Instantiate(CosmeticShopItemUIPrefab, TopsItemRect).GetComponent<CosmeticShopItemUI>();
                    break;
                case COSMETIC_TYPE.BOTTOM:
                    itemUI = Instantiate(CosmeticShopItemUIPrefab, BottomsItemRect).GetComponent<CosmeticShopItemUI>();
                    break;
                case COSMETIC_TYPE.SHOE:
                    itemUI = Instantiate(CosmeticShopItemUIPrefab, ShoeItemRect).GetComponent<CosmeticShopItemUI>();
                    break;
            }
            itemUI.Init(cosmeticInfo.Key, SetSelectedShopitem);

            // Set Current Selected item
            if (currSelectedShopItem == null)
                currSelectedShopItem = itemUI;

           
        }
    }

    // Called when a shop item ui is clicked on
    private void SetSelectedShopitem(CosmeticShopItemUI item)
    {
        // Disable Selection UI on current selected ui
        if (currSelectedShopItem != null)
            currSelectedShopItem.SetSelected(false);
        // Set New UI
        currSelectedShopItem = item;
        // Enable New UI
        currSelectedShopItem.SetSelected(true);
    }

    public void PurchaseCosmetic()
    {
        if (currSelectedShopItem == null)
            return;

        Cosmetic cosmetic = currSelectedShopItem.CosmeticInfo;

        // Check if enough tokens
        if (cosmetic.CosmeticPrice > PlayerData.NumTokens)
        {
            // show warning
            return;
        }

        // Update Tokens
        CurrencyManager.Instance.UpdateTokenAmount(-1 * cosmetic.CosmeticPrice);
        PopUpManager.Instance.AddCurrencyPopUp(CURRENCY_TYPE.TOKEN, -1 * cosmetic.CosmeticPrice, currSelectedShopItem.transform.position);
        // Unlock Cosmetic
        //CosmeticManager.Instance.
        PlayerData.SetCosmeticUnlocked(cosmetic);
        AvatarCustomisationManager.Instance.UpdateInventoryRect(cosmetic.CosmeticBodyPartType);
        // Update Player Stat
        PlayerStatsManager.Instance.UpdatePlayerStat(PLAYER_STAT.COSMETICS_OWNED, ++PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.COSMETICS_OWNED]);

        currSelectedShopItem = null;
        UpdateShopTabs();

        // Confirmation ui to say purchase successful

    }

    public void SetSelectedTab(int index) // Called In Inspector on Tab Clicked
    {
        // Do Nothing if its current tab
        if (CurrentlySelectedindex == index)
            return;

        // Disable Current Tab Selection UI And ShopitemPanels
        ShopItemPanels[CurrentlySelectedindex].SetActive(false);
        ShopSelectedTabs[CurrentlySelectedindex].SetActive(false);

        // Set New Tab
        CurrentlySelectedindex = index;

        // Enable Current Tab Selection UI And ShopitemPanels
        ShopItemPanels[CurrentlySelectedindex].SetActive(true);
        ShopSelectedTabs[CurrentlySelectedindex].SetActive(true);
    }
}
