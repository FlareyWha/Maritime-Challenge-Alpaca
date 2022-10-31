using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BaseProjectile : NetworkBehaviour
{
    [SyncVar (hook =nameof(ToggleVisibility))]
    public bool active = false;

    protected Vector2 velocity = Vector2.zero;
    protected float homing_rate = 20.0f;

    protected float INITIAL_SPEED = 15.0f;
    protected float SPEED = 15.0f;
    protected float accel_rate = 0.2f;

    protected Rigidbody2D rb = null;

    protected Player ownerPlayer = null;
    protected BaseEnemy target = null;

    protected int damage = 0;
    private float lifetime = 10.0f;
    private float lifetime_timer = 10.0f;




    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(active);
    }

    public override void OnStartServer()
    {
        active = false;
    }

    public virtual void FixedUpdate()
    {
        if (!isServer)
            return;

        // Update Position
        if (target != null)
        {
            if (!target.gameObject.activeSelf)
            {
                target = null;
                return;
            }

            Vector2 dis = target.TargetTransform.position - transform.position;
            Vector2 homingDir = dis.normalized - velocity.normalized;
            velocity += homingDir * homing_rate * Time.deltaTime;
        }

        lifetime_timer -= Time.deltaTime;

        SPEED += accel_rate * Time.deltaTime;
        rb.position += velocity.normalized * SPEED * Time.deltaTime;

        if (lifetime_timer <= 0.0f)
        {
            //NetworkServer.Destroy(this.gameObject);
            Deactivate();
        }
    }

    [Server]
    public void Activate()
    {
        active = true;
        gameObject.SetActive(true);
        Debug.Log("Projectile Activated.");

        lifetime_timer = lifetime;
        SPEED = INITIAL_SPEED;
    }

    [Server]
    public void Deactivate()
    {
        active = false;
        gameObject.SetActive(false);
    }

   
    private void ToggleVisibility(bool _old, bool _new)
    {
        gameObject.SetActive(_new);
    }
}


