using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleshipShopItemUI : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject SelectOverlay;
    [SerializeField]
    private Image ShipDisplayImage;
    [SerializeField]
    private Text CostText;


    private BattleshipInfo battleshipInfo;
    public BattleshipInfo BattleshipInfo { get { return battleshipInfo; } }

    private Action<BattleshipShopItemUI> OnShopItemClicked;

    private void Awake()
    {
        button.onClick.AddListener(OnItemClicked);
    }

    public void Init(BattleshipInfo shipInfo, Action<BattleshipShopItemUI> action)
    {
        battleshipInfo = shipInfo;

        ShipDisplayImage.sprite = shipInfo.BattleshipData.ShopIconSprite;
        CostText.text = shipInfo.BattleshipData.Cost.ToString();


        OnShopItemClicked = action;
    }

    private void OnItemClicked()
    {
        OnShopItemClicked?.Invoke(this);
    }

    public void SetSelected(bool selected)
    {
        SelectOverlay.SetActive(selected);
    }
}
