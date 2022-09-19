using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sit : Interactable
{
    bool isSeated = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdateInteractMessage();
    }

    public override void Interact()
    {
        if (isSeated)
        {
            //Prob clamp the player pos to the seat or smth and prevent movment
        }
        else
        {

            //Do the opposite of above
        }

        isSeated = !isSeated;
        UpdateInteractMessage();
    }

    private void UpdateInteractMessage()
    {
        if (isSeated)
            interactMessage = "Sit Down?";
        else
            interactMessage = "Stand Up?";
    }
}
