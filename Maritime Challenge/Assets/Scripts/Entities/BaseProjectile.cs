using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BaseProjectile : NetworkBehaviour
{
    protected Vector2 velocity = Vector2.zero;
    protected float homing_rate = 30.0f;

    protected float SPEED = 15.0f;
    protected float accel_rate = 0.5f;

    protected Rigidbody2D rb = null;

    protected Player ownerPlayer = null;
    protected BaseEnemy target = null;

    protected int damage = 0;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void FixedUpdate()
    {

        if (!isServer)
            return;

        // Update Position
        if (target != null)
        {
            Vector2 dis = target.TargetTransform.position - transform.position;
            Vector2 homingDir = dis.normalized - velocity.normalized;
            velocity += homingDir * homing_rate * Time.deltaTime;
        }

        SPEED += accel_rate * Time.deltaTime;
        rb.position += velocity.normalized * SPEED * Time.deltaTime;
    }
}
