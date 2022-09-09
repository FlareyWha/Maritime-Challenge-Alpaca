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

        Debug.Log("Interactable: SetMyPlayer!");
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
        if (collision.gameObject == myPlayer.gameObject || collision.gameObject == myPlayer.GetBattleShip().gameObject)
        {
            if (PlayerInteract.InRangeList.Count == 0)
            {
                // Enable Interact Button if no others
                UIManager.Instance.EnableInteractButton(interactMessage);
            }

            PlayerInteract.InRangeList.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == myPlayer.gameObject || collision.gameObject == myPlayer.GetBattleShip().gameObject)
        {
            PlayerInteract.InRangeList.Remove(this);
            if (PlayerInteract.InRangeList.Count == 0)
            {
                // Disable Interact Button if no others
                UIManager.Instance.DisableInteractButton();
            }
        }
    }
}
