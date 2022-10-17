using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingMall : BaseInteractable
{
    void Start()
    {
        interactMessage = "Enter Shopping Mall?";
    }

    public override void Interact()
    {
        PlayerData.CommandsHandler.SwitchSubScene("ShoppingMallScene", SceneManager.GetSpawnPos(SPAWN_POS.SHOPPINGMALL_ENTRANCE));

    }
}
