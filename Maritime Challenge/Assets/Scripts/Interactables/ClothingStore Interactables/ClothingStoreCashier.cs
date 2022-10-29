using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingStoreCashier : BaseInteractable
{
    [SerializeField]
    private GameObject ClothingStoreUIPanel;

    void Start()
    {
        interactMessage = "Buy Cosmetics?";
    }

 
    public override void Interact()
    {
        ClothingStoreUIPanel.SetActive(true);

        UIManager.Instance.DisableInteractButton();
    }

}
