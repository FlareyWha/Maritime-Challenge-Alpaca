using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CannonBall : NetworkBehaviour
{

    private Vector2 velocity = Vector2.zero;
    private float homing_rate = 3.0f;

    private float SPEED = 20.0f;
    private float accel_rate = 0.5f;

    private Rigidbody2D rb = null;

    private GameObject target = null;

    [Server]
    public void Init(GameObject target)
    {
        rb = GetComponent<Rigidbody2D>();
        this.target = target;
        velocity = (target.transform.position - transform.position).normalized * SPEED;
    }

    private void FixedUpdate()
    {
        if (!isServer)
            return;

        Vector2 dis = target.transform.position - transform.position;
        Vector2 homingDir = dis.normalized - velocity.normalized;
        velocity += homingDir * homing_rate * Time.deltaTime;

        SPEED += accel_rate * Time.deltaTime;
        rb.position += velocity.normalized * SPEED * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer)
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            NetworkServer.Destroy(gameObject);
        }
    }


}
