using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipStore : BaseInteractable
{
    [SerializeField]
    private GameObject StoreUIPanel;

    void Start()
    {
        interactMessage = "Visit Battleship Store?";
    }

    public override void Interact()
    {
        StoreUIPanel.SetActive(true);
        UIManager.Instance.ToggleJoystick(false);
        UIManager.Instance.DisableInteractButton();
    }

    public void OnCloseStorePanel()
    {
        UIManager.Instance.EnableInteractButton(interactMessage);
        UIManager.Instance.ToggleJoystick(true);
    }
}
