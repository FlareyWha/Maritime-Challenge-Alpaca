using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
   // [SerializeField]
   // private float InteractRadius = 3.0f;


    private Player myPlayer = null;
    protected string interactMessage = "Interact";
   // protected bool in_range = false;
 


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

    void Update()
    {
       
    }

    public virtual void Interact()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (myPlayer == null || myPlayer.GetBattleShip() == null)// - INEFFICIENT TEMP FIX COS ANNOYING
            return;

        if (collision.gameObject == myPlayer.gameObject || collision.gameObject == myPlayer.GetBattleShip().gameObject)
        {
            PlayerInteract.OnEnterInteractable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
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
