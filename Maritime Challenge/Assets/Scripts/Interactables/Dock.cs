using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : BaseInteractable
{

    [SerializeField]
    private bool IsStartingDock = false;
    [SerializeField]
    private Transform RefShipTransform, RefPlayerTransform;

    private bool is_docked_here = false;

    private bool interacted;
    public bool Interacted
    {
        get { return interacted; }
        private set { }
    }

    void Start()
    {
        if (IsStartingDock)
            is_docked_here = true;

        UpdateInteractMessage();


    }

    private void Update()
    {
        if (interacted)
            interacted = false;
    }

    public override void Interact()
    {
        if (is_docked_here)
        {
            PlayerData.MyPlayer.SummonBattleShip(this);
            interacted = true;
        }
        else
            PlayerData.MyPlayer.DockShip(this);

        is_docked_here = !is_docked_here;
        UpdateInteractMessage();
    }

    private void UpdateInteractMessage()
    {
        if (is_docked_here)
            interactMessage = "Summon Ship?";
        else
            interactMessage = "Dock Here?";
    }

    public Transform GetRefShipTransform()
    {
        return RefShipTransform;
    }

    public Transform GetRefPlayerTransform()
    {
        return RefPlayerTransform;
    }
}
