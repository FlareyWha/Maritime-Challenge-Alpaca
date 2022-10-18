using System;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField]
    private Image TitleArt;
    [SerializeField]
    private GameObject LockIcon, EquippedOverlay;
    [SerializeField]
    private Button button;

    public int SortOrderRef = 0;

    private Title title;
    public Title LinkedTitle { get { return title;  } }

    private Color32 lockedColor = new Color32(100, 100, 120, 255);

    private Action<TitleUI> onSelectAction;

    private void Awake()
    {
        button.onClick.AddListener(OnTitleSelected);
    }

    public void Init(Title title, bool unlocked, Action<TitleUI> action)
    {
        this.title = title;
        TitleArt.sprite = title.LinkedTitle.TitleSprite;

        button.interactable = unlocked;

        LockIcon.SetActive(!unlocked);
        if (unlocked)
            TitleArt.color = Color.white;
        else
            TitleArt.color = lockedColor;

        onSelectAction = action;
    }


    private void OnTitleSelected()
    {
        ToggleEquippedOverlay(true);
        onSelectAction?.Invoke(this);
    }

    public void ToggleEquippedOverlay(bool show)
    {
        EquippedOverlay.SetActive(show);
    }

  
}
