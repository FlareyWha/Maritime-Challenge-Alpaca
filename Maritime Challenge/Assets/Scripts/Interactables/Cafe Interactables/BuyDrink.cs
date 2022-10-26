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
        CafeManager.Instance.HasDrink = true;
        UIManager.Instance.DisableInteractButton();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (myPlayer == null || myPlayer.GetBattleShip() == null)// - INEFFICIENT TEMP FIX COS ANNOYING
            return;

        if (!CafeManager.Instance.HasDrink && (collision.gameObject == myPlayer.gameObject || collision.gameObject == myPlayer.GetBattleShip().gameObject))
        {
            PlayerInteract.OnEnterInteractable(this);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (myPlayer == null || myPlayer.GetBattleShip() == null)// - INEFFICIENT TEMP FIX COS ANNOYING
            return;

        if (collision.gameObject == myPlayer.gameObject || collision.gameObject == myPlayer.GetBattleShip().gameObject)
        {
            PlayerInteract.OnLeaveInteractable(this);
        }
    }
}
