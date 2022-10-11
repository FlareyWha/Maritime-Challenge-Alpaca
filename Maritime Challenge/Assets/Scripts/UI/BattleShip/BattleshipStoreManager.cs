using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleshipStoreManager : MonoBehaviour
{
    [SerializeField]
    private GameObject BattleshipShopItemUIPrefab, BattleshipUIPrefab;
    [SerializeField]
    private Transform ShopRect, OwnedRect;
    [SerializeField]
    private Text OwnedTokensText;
    [SerializeField]
    private Text ShopKeeperQuoteText, BattleshipNameText;
    [SerializeField]
    private Text HPText, ATKText, ATKSPDText, MOVSPDText;

    private BattleshipShopItemUI currSelectedShopItem = null;
    private BattleshipUI currSelectedBattleship = null;


    private void Start()
    {
        PlayerData.OnNumTokensUpdated += UpdateNumTokensUI;
        UpdateNumTokensUI();
        UpdateShopItemsDisplay();
        UpdateOwnedShipsDisplay();
    }

    private void UpdateNumTokensUI()
    {
        OwnedTokensText.text = PlayerData.NumTokens.ToString();
    }

    private void UpdateShopItemsDisplay()
    {
        // Clear Rect
        foreach (Transform child in ShopRect)
        {
            Destroy(child.gameObject);
        }

        // Init Rect
        foreach (KeyValuePair<BattleshipInfo, bool> shipInfo in PlayerData.BattleshipList)
        {
            // If Owned, Dont Show in Shop
            if (shipInfo.Value)
                continue;

            // Instantiate UI
            BattleshipShopItemUI itemUI = Instantiate(BattleshipShopItemUIPrefab, ShopRect).GetComponent<BattleshipShopItemUI>();
            itemUI.Init(shipInfo.Key, SetSelectedShopUI);

            // Set Current to First
            if (currSelectedShopItem == null)
            {
                currSelectedShopItem = itemUI;
                itemUI.SetSelected(true);
            }
        }
    }

    private void UpdateOwnedShipsDisplay()
    {
        // Clear Rect
        foreach (Transform child in OwnedRect)
        {
            Destroy(child.gameObject);
        }

        // Init Rect
        foreach (KeyValuePair<BattleshipInfo, bool> shipInfo in PlayerData.BattleshipList)
        {
            // If Not Owned, Dont Show in Inventory
            if (!shipInfo.Value)
                continue;

            // Instantiate UI
            BattleshipUI itemUI = Instantiate(BattleshipUIPrefab, OwnedRect).GetComponent<BattleshipUI>();
            itemUI.Init(shipInfo.Key, SetSelectedInventoryUI);

            // Check If Equipped
            if (shipInfo.Key.BattleshipID == PlayerData.CurrentBattleship)
            {
                currSelectedBattleship = itemUI;
                itemUI.SetSelected(true);
            }
        }
    }


    private void SetSelectedShopUI(BattleshipShopItemUI ui)
    {
        if (currSelectedShopItem != null)
            currSelectedShopItem.SetSelected(false);
        currSelectedShopItem = ui;
        currSelectedShopItem.SetSelected(true);

        SetShipDetailsUI(ui.BattleshipInfo);
    }

    private void SetSelectedInventoryUI(BattleshipUI ui)
    {
        if (currSelectedBattleship != null)
            currSelectedBattleship.SetSelected(false);
        currSelectedBattleship = ui;
        currSelectedBattleship.SetSelected(true);

        SetShipDetailsUI(ui.BattleshipInfo);
    }

    private void SetShipDetailsUI(BattleshipInfo shipInfo)
    {
        ShopKeeperQuoteText.text = "\"" + shipInfo.BattleshipData.ShipDescription + "\"";
        BattleshipNameText.text = shipInfo.BattleshipName;

        HPText.text = shipInfo.HP.ToString();
        ATKText.text = shipInfo.Atk.ToString();
        ATKSPDText.text = "1 ball/" + shipInfo.AtkSpd.ToString() + "s";
        MOVSPDText.text = shipInfo.MoveSpd.ToString();
    }

    public void BuyItem()
    {
        if (currSelectedShopItem == null)
            return;


        int price = currSelectedShopItem.BattleshipInfo.BattleshipData.Cost;
        int id = currSelectedShopItem.BattleshipInfo.BattleshipID;
        if (PlayerData.NumTokens >= price)
        {
            BattleshipsManager.Instance.UpdateTokenAmount(-1 * price);
            BattleshipsManager.Instance.UnlockBattleship(id);
            PlayerData.SetBattleshipUnlocked(currSelectedShopItem.BattleshipInfo);
            UpdateShopItemsDisplay();
            UpdateOwnedShipsDisplay();

            Debug.Log("Battleship Bought! : " + currSelectedShopItem.BattleshipInfo.BattleshipName);
            if (ShopRect.childCount > 0)
                currSelectedShopItem = ShopRect.GetChild(0).gameObject.GetComponent<BattleshipShopItemUI>();
            else
                currSelectedShopItem = null;
            //Confirmation text to say purchase successful
        }
        else
        {
            //Confirmation text to say purchase not successful
        }

    }

    public void ChangeBattleship()
    {
        BattleshipsManager.Instance.UpdateCurrentBattleship(currSelectedBattleship.BattleshipInfo.BattleshipID);

        // Change SHip Logic ...
        PlayerData.MyPlayer.GetBattleShip().ChangeBattleShip(currSelectedBattleship.BattleshipInfo.BattleshipID);
        Debug.Log("Changed Battleship to " + currSelectedBattleship.BattleshipInfo.BattleshipName);
    }
}
