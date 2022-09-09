using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Battleship : NetworkBehaviour
{

    private Rigidbody2D rb = null;

    private Vector2 velocity = Vector2.zero;
    private Vector2 accel = Vector2.zero;
    private float accel_rate = 10.0f;
    private float deccel_rate = 5.0f;

    private const float MAX_VEL = 10.0f;

    public override void OnStartAuthority()
    {
        rb = GetComponent<Rigidbody2D>();

        Debug.Log("BattleShip: Taken Authority Over BattleShip");
        PlayerData.MyPlayer.SetLinkedShip(this);
        gameObject.SetActive(false);
    }

   

    void Update()
    {
        if (!hasAuthority)
            return;

        if (UIManager.Instance.Joystick.GetDirection() != Vector2.zero)
        {
            accel = UIManager.Instance.Joystick.GetDirection();
            velocity += accel * accel_rate * Time.deltaTime;
        }

        if (velocity.magnitude != 0 && 
            Vector2.Dot(accel, velocity) < 0)
        {
            velocity = Vector2.zero;
        }

        if (velocity.magnitude != 0)
        {
            float currSpeed = velocity.magnitude;
            float newSpeed = currSpeed - deccel_rate * Time.deltaTime;
            velocity = velocity.normalized * newSpeed;
        }

        velocity.x = Mathf.Clamp(velocity.x, -MAX_VEL, MAX_VEL);
        velocity.y = Mathf.Clamp(velocity.y, -MAX_VEL, MAX_VEL);

        rb.position += velocity * Time.deltaTime;

    }


    public void Dock()
    {
        gameObject.SetActive(false);
    }
    public void Summon(Transform refTransform)
    {

    }
}

