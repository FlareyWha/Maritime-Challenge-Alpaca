using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CannonBall : NetworkBehaviour
{

    private Vector2 velocity = Vector2.zero;
    private float homing_rate = 2.0f;

    private float SPEED = 15.0f;
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

        // Update Rotation
        if (isClient)
        {
            Vector3 rot = transform.rotation.eulerAngles;
            rot.z += 20.0f * Time.deltaTime;
            if (rot.z > 360)
                rot.z -= 360;
            transform.rotation = Quaternion.Euler(rot);
        }


        if (!isServer)
            return;

        // Update Position
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
