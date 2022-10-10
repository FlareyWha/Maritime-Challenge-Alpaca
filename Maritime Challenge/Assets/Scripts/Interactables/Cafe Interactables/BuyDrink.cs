using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyDrink : BaseInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        interactMessage = "Buy Drink?";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        Debug.Log("Bought drink");
    }
}
