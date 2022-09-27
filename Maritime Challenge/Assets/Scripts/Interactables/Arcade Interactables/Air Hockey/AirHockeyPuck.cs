using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AirHockeyPuck : NetworkBehaviour
{

    private Rigidbody2D rb = null;

 
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!isServer)
            rb.isKinematic = true;
    }

    [Server]
    public void ForceStop()
    {
        rb.velocity = Vector2.zero;
    }

   
}
