using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerSetOrder : MonoBehaviour
{
    [SerializeField]
    private int orderInLayer;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player == null)
                return;
            player.SetOrderInLayer(orderInLayer);
        }
    }
}
