using System;
using UnityEngine;
using UnityEngine.UI;

public class RedemptionItemUI : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private Text CostText;
    [SerializeField]
    private Image VoucherBG;

    private RedeemableItemSO item;

    private Action<RedeemableItemSO> exchangeAction;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Init(RedeemableItemSO item, Action<RedeemableItemSO> action)
    {
        this.item = item;
        VoucherBG.sprite = item.VoucherSprite;
        CostText.text = item.RollarsCost.ToString();

        exchangeAction = action;
    }

    private void OnButtonClicked()
    {
        exchangeAction.Invoke(item);
    }

}
