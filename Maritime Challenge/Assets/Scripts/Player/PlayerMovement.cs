using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    private const float WALK_SPEED = 3.0f;

    public override void OnStartLocalPlayer()
    {
        // Init Player to SpawnPos

        // Attach Camera
        UIManager.Instance.Camera.SetFollowTarget(gameObject);

    }

  
    void Update()
    {
        if (!isLocalPlayer)
            return;

        Vector2 input = UIManager.Instance.Joystick.GetDirection();
        transform.position += new Vector3(input.x, input.y, 0.0f) * WALK_SPEED * Time.deltaTime;
        
    }
}
