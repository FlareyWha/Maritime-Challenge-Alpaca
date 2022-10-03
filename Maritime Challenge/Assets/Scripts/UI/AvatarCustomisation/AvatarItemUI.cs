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
        onSelectAction?.Invoke(item);
    }
}
