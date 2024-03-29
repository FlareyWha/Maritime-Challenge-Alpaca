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

    public static Dock StartingDock = null;

    void Start()
    {
        if (IsStartingDock)
        {
            is_docked_here = true;
            StartingDock = this;
        }

        UpdateInteractMessage();


    }

    private void Update()
    {
        
    }

    public override void Interact()
    {
        if (is_docked_here)
        {
            PlayerData.MyPlayer.SummonBattleShip(this);
        }
        else
            PlayerData.MyPlayer.DockShip(this);

        is_docked_here = !is_docked_here;
        UpdateInteractMessage();

        interacted = true;
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
