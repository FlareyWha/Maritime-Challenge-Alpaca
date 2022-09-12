using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
  
    private const float WALK_SPEED = 5.0f;

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

        rb.position += input * WALK_SPEED * Time.deltaTime;
    }

}
