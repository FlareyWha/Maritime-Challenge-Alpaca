using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CannonBall : NetworkBehaviour
{

    private Rigidbody2D rb = null;


    private void FixedUpdate()
    {
        if (!isServer)
            return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }


}
