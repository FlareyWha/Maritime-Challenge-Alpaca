using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arcade : BaseInteractable
{
    void Start()
    {
        interactMessage = "Enter Arcade?";
    }

    public override void Interact()
    {
        PlayerData.CommandsHandler.SwitchSubScene("ArcadeScene", SceneManager.GetSpawnPos(SPAWN_POS.ARCADE_ENTRANCE));

    }
}
