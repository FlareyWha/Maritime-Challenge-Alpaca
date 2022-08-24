using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float WALK_SPEED = 3.0f;

    void Start()
    {
        // Init Player to SpawnPos
    }


    void Update()
    {
        Vector2 input = UIManager.Instance.Joystick.GetDirection();
        transform.position += new Vector3(input.x, input.y, 0.0f) * WALK_SPEED * Time.deltaTime;
        
    }
}
