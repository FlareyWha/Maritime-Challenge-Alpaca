using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cafe : Interactable
{

    void Start()
    {
        interactMessage = "Enter cafe?";
    }

    public override void Interact()
    {
        Debug.Log("Cafe: Interact function called");

        PlayerData.CommandsHandler.SwitchSubScene("CafeScene", SceneManager.EnterCafeSpawnPos);
       
    }
}
