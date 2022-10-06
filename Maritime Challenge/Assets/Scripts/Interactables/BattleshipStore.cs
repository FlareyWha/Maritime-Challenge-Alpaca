using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipStore : Interactable
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
    }
}
