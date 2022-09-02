using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
  
    //private const float WALK_SPEED = 3.0f;

    private Vector2 velocity = Vector2.zero;
    private float accel_rate = 1.0f;
    private float deccel_rate = 1.0f;

    private const float MAX_VEL = 3.0f;

    private Rigidbody2D rb = null;

    public override void OnStartClient()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        Vector2 input = UIManager.Instance.Joystick.GetDirection();


        rb.position += input * MAX_VEL * Time.deltaTime;
    }

}
