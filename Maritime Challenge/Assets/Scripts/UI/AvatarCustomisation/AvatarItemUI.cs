using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarItemUI : MonoBehaviour
{
    [SerializeField]
    private Button selectButton;
    [SerializeField]
    private Image itemIcon;
    [SerializeField]
    private GameObject equippedOverlay;

    private AvatarCosmetic item;

    private Action<AvatarCosmetic> onSelectAction;


    private void Start()
    {
        selectButton.onClick.AddListener(OnItemSelected);
    }


    public void Init(AvatarCosmetic cosmetic, Action<AvatarCosmetic> action)
    {
        item = cosmetic;
        itemIcon.sprite = cosmetic.IconSprite;
        onSelectAction = action;
    }

    private void OnItemSelected()
    {
        SetEquippedOverlay(true);
        onSelectAction?.Invoke(item);
    }

    public void SetEquippedOverlay(bool show)
    {
        equippedOverlay.SetActive(show);
    }
}
