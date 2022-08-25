using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField]
    private Text PlayerDisplayName;
    private int PlayerID;

    //private const float WALK_SPEED = 3.0f;

    private Vector2 velocity = Vector2.zero;
    private float accel_rate = 1.0f;
    private float deccel_rate = 1.0f;

    private const float MAX_VEL = 3.0f;


    void Update()
    {
        if (!isLocalPlayer)
            return;

        Vector2 input = UIManager.Instance.Joystick.GetDirection();
        transform.position += new Vector3(input.x, input.y, 0.0f) * MAX_VEL * Time.deltaTime;
        
    }
}
