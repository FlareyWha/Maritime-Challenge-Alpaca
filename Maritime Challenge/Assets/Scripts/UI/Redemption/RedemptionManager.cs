using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedemptionManager : MonoBehaviour
{
    [SerializeField]
    private List<RedeemableItemSO> RedeemableItemsList;

    [SerializeField]
    private GameObject RedemptionItemUIPrefab;
    [SerializeField]
    private Transform VouchersRect;

    [SerializeField]
    private Text NumRollarsText;

    private void Awake()
    {
        UpdateNumRollarsUI();
        PlayerData.OnNumRollarsUpdated += UpdateNumRollarsUI;

        // Clear
        foreach (Transform child in VouchersRect)
        {
            Destroy(child.gameObject);
        }
        foreach (RedeemableItemSO item in RedeemableItemsList)
        {
            RedemptionItemUI redeemUI = Instantiate(RedemptionItemUIPrefab, VouchersRect).GetComponent<RedemptionItemUI>();
            redeemUI.Init(item, ExchangeItem);
        }
    }

    private void ExchangeItem(RedeemableItemSO item)
    {
        if (PlayerData.NumRightshipRollars < item.RollarsCost)
        {
            // ui not enough rollars
            return;
        }

        // Redeem Logic

    }

    private void UpdateNumRollarsUI()
    {
        NumRollarsText.text = PlayerData.NumRightshipRollars.ToString();
    }
}
