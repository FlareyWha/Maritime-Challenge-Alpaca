using System;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticShopItemUI : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject SelectOverlay;
    [SerializeField]
    private Image CosmeticDisplayImage;
    [SerializeField]
    private Text CostText;


    private Cosmetic cosmeticInfo;
    public Cosmetic CosmeticInfo { get { return cosmeticInfo; } }

    private Action<CosmeticShopItemUI> OnShopItemClicked;

    private void Awake()
    {
        button.onClick.AddListener(OnItemClicked);
    }

    public void Init(Cosmetic cosmetic, Action<CosmeticShopItemUI> action)
    {
        cosmeticInfo = cosmetic;

        CosmeticDisplayImage.sprite = cosmetic.LinkedCosmetic.IconSprite;
        CostText.text = cosmetic.CosmeticPrice.ToString();

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
