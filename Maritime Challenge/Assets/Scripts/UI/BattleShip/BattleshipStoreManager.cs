using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip purchaseSucessfulClip, purchaseFailedClip;


    private void Awake()
    {
        PlayerData.OnNumTokensUpdated += UpdateNumTokensUI;
        UpdateNumTokensUI();
        UpdateShopItemsDisplay();
        UpdateOwnedShipsDisplay();
        SetDetailsToCurrentShopItem();
    }

    private void OnDestroy()
    {
        PlayerData.OnNumTokensUpdated -= UpdateNumTokensUI;
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

    private void SetEmptyDetailsUI()
    {
        ShopKeeperQuoteText.text = "Nothing to view here.";
        BattleshipNameText.text = "";
        HPText.text = "";
        ATKText.text = "";
        ATKSPDText.text = "";
        MOVSPDText.text = "";
    }

    public void SetDetailsToCurrentInventory()
    {
        if (currSelectedBattleship == null)
        {
            SetEmptyDetailsUI();
            return;
        }

        SetShipDetailsUI(currSelectedBattleship.BattleshipInfo);
    }

    public void SetDetailsToCurrentShopItem()
    {
        if (currSelectedShopItem == null)
        {
            SetEmptyDetailsUI();
            return;
        }

        SetShipDetailsUI(currSelectedShopItem.BattleshipInfo);
    }

    public void BuyItem()
    {
        if (currSelectedShopItem == null)
        {
            audioSource.clip = purchaseFailedClip;
            audioSource.Play();
            return;
        }

        int price = currSelectedShopItem.BattleshipInfo.BattleshipData.Cost;
        int id = currSelectedShopItem.BattleshipInfo.BattleshipID;
        if (PlayerData.NumTokens >= price)
        {
            // Update Tokens
            CurrencyManager.Instance.UpdateTokenAmount(-1 * price);
            PopUpManager.Instance.AddCurrencyPopUp(CURRENCY_TYPE.TOKEN, -1 * price, currSelectedShopItem.transform.position);
            // Update Battleship
            BattleshipsManager.Instance.UnlockBattleship(id);
            PlayerData.SetBattleshipUnlocked(currSelectedShopItem.BattleshipInfo);
            PlayerStatsManager.Instance.UpdatePlayerStat(PLAYER_STAT.BATTLESHIPS_OWNED, ++PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.BATTLESHIPS_OWNED]);

            UpdateShopItemsDisplay();
            UpdateOwnedShipsDisplay();

            if (ShopRect.childCount > 0)
                currSelectedShopItem = ShopRect.GetChild(0).gameObject.GetComponent<BattleshipShopItemUI>();
            else
                currSelectedShopItem = null;
            //Confirmation text to say purchase successful

            audioSource.clip = purchaseSucessfulClip;
        }
        else
        {
            //Confirmation text to say purchase not successful
            audioSource.clip = purchaseFailedClip;
        }

        audioSource.Play();
    }

    public void ChangeBattleship()
    {
        BattleshipsManager.Instance.UpdateCurrentBattleship(currSelectedBattleship.BattleshipInfo.BattleshipID);

        // Change SHip Logic ...
        PlayerData.MyPlayer.GetBattleShip().SyncBattleshipSprites(currSelectedBattleship.BattleshipInfo.BattleshipID);
        Debug.Log("Changed Battleship to " + currSelectedBattleship.BattleshipInfo.BattleshipName);
    }
}
