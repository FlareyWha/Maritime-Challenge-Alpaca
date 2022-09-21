using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cafe : Interactable
{

    void Start()
    {
        interactMessage = "Enter Cafe?";
    }

    public override void Interact()
    {
        PlayerData.CommandsHandler.SwitchSubScene("CafeScene", SceneManager.EnterCafeSpawnPos);
       
    }
}
