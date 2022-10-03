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


    public void Init(Action<AvatarCosmetic> action)
    {
        onSelectAction = action;
    }

    private void OnItemSelected()
    {
        onSelectAction?.Invoke(item);
    }
}
