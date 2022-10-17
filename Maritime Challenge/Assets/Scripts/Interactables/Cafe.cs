using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cafe : BaseInteractable
{
    void Start()
    {
        interactMessage = "Enter Cafe?";
    }

    public override void Interact()
    {
        PlayerData.CommandsHandler.SwitchSubScene("CafeScene", SceneManager.GetSpawnPos(SPAWN_POS.CAFE_ENTRANCE));
       
    }
}
