using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractable : MonoBehaviour
{
    protected Player myPlayer = null;
    protected string interactMessage = "Interact";

    void Awake()
    {
        StartCoroutine(InteractableInits());
    }

    IEnumerator InteractableInits()
    {
        while (PlayerData.MyPlayer == null)
            yield return null;

        myPlayer = PlayerData.MyPlayer;
    }

    public virtual void Interact()
    {

    }

    protected virtual bool CheckRequirements()
    {
        return true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (myPlayer == null || myPlayer.GetBattleShip() == null)// - INEFFICIENT TEMP FIX COS ANNOYING
            return;

        if (CheckRequirements() && collision.gameObject == myPlayer.gameObject || collision.gameObject == myPlayer.GetBattleShip().gameObject)
        {
            PlayerInteract.OnEnterInteractable(this);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    { 
    
        if (myPlayer == null || myPlayer.GetBattleShip() == null)// - INEFFICIENT TEMP FIX COS ANNOYING
            return;

        if (collision.gameObject == myPlayer.gameObject || collision.gameObject == myPlayer.GetBattleShip().gameObject)
        {
            PlayerInteract.OnLeaveInteractable(this);
        }
    }

    public string GetInteractMessage()
    {
        return interactMessage;
    }
}
