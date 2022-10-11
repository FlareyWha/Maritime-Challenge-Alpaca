using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : BaseInteractable
{
    [SerializeField]
    private string InteractMessageText, TargetScene;

    void Start()
    {
        interactMessage = InteractMessageText;
    }

    public override void Interact()
    {
        PlayerData.CommandsHandler.SwitchSubScene(TargetScene, SceneManager.ExitCafeSpawnPos);
    }
}
