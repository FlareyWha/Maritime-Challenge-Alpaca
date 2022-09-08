using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
   // [SerializeField]
   // private float InteractRadius = 3.0f;


    private Player myPlayer = null;

   // protected bool in_range = false;
 


    void Start()
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
        if (collision.gameObject == PlayerData.MyPlayer.gameObject)
        {
            // Enable Interact Button
            // ...

            PlayerInteract.InRangeList.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerData.MyPlayer.gameObject || collision.gameObject == PlayerData.MyPlayer.GetBattleShip().gameObject)
        {
            PlayerInteract.InRangeList.Remove(this);
            if (PlayerInteract.InRangeList.Count == 0)
            {

                // Disable Interact Button if no others
            }
        }
    }
}
