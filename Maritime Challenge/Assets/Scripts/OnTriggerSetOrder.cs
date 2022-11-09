using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerSetOrder : MonoBehaviour
{
    [SerializeField]
    private int orderInLayer;

    public int OrderInLayer { get { return orderInLayer; } }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerOrderInLayerTrigger player = collision.gameObject.GetComponent<PlayerOrderInLayerTrigger>();
            if (player == null)
                return;

            player.AddTrigger(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerOrderInLayerTrigger player = collision.gameObject.GetComponent<PlayerOrderInLayerTrigger>();
            if (player == null)
                return;

            player.RemoveTrigger(this);
        }
    }
}
