using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBoss : BaseInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        interactMessage = "Fight World Boss";
    }

    public override void Interact()
    {
        //Enter world boss scene or wtv
        //PlayerData.CommandsHandler.SwitchSubScene("WorldHubScene", SceneManager.StartWorldHubSpawnPos);
    }
}
