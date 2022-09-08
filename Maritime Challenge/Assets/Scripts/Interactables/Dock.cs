using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : Interactable
{


    [SerializeField]
    private Transform RefShipTransform, RefPlayerTransform;

    private bool is_docked_here = false;

    public override void Interact()
    {
        
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
