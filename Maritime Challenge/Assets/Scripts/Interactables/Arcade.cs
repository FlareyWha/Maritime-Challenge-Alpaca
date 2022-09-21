using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arcade : Interactable
{
    void Start()
    {
        interactMessage = "Enter Arcade?";
    }

    public override void Interact()
    {
        PlayerData.CommandsHandler.SwitchSubScene("ArcadeScene", SceneManager.EnterArcadeSpawnPos);

    }
}
