using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleshipUI : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject SelectOverlay;
    [SerializeField]
    private Image ShipDisplayImage;


    private BattleshipInfo battleshipInfo;
    public BattleshipInfo BattleshipInfo { get { return battleshipInfo; } }

    private Action<BattleshipUI> OnShopItemClicked;

    private void Awake()
    {
        button.onClick.AddListener(OnItemClicked);
    }

    public void Init(BattleshipInfo shipInfo, Action<BattleshipUI> action)
    {
        battleshipInfo = shipInfo;

        ShipDisplayImage.sprite = shipInfo.BattleshipData.ShopIconSprite;

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
