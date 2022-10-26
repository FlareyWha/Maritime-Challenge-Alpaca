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

    public RedeemableItemSO Item;

    private Action<RedemptionItemUI> exchangeAction;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Init(RedeemableItemSO item, Action<RedemptionItemUI> action)
    {
        this.Item = item;
        VoucherBG.sprite = item.VoucherSprite;
        CostText.text = item.RollarsCost.ToString();

        exchangeAction = action;
    }

    private void OnButtonClicked()
    {
        exchangeAction.Invoke(this);
    }

}
