using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CannonBall : NetworkBehaviour
{

    private Vector2 velocity = Vector2.zero;
    private float homing_rate = 30.0f;

    private float SPEED = 15.0f;
    private float accel_rate = 0.5f;

    private Rigidbody2D rb = null;

    private Player ownerPlayer = null;
    private BaseEnemy target = null;

    private int damage = 10;

    void Awake()
    {
        if (isClient)
            gameObject.SetActive(false);
    }

    [Server]
    public void Init(GameObject target, Player owner)
    {
        rb = GetComponent<Rigidbody2D>();
        this.target = target.GetComponent<BaseEnemy>();
        this.ownerPlayer = owner;
        velocity = (target.transform.position - transform.position).normalized * SPEED;

        target.GetComponent<BaseEntity>().OnEntityDied += OnTargetDiedCallback;
        Show();
    }
    
    [ClientRpc]
    private void Show()
    {
        gameObject.SetActive(true);
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
        if (target != null)
        {
            Vector2 dis = target.TargetTransform.position - transform.position;
            Vector2 homingDir = dis.normalized - velocity.normalized;
            velocity += homingDir * homing_rate * Time.deltaTime;
        }

        SPEED += accel_rate * Time.deltaTime;
        rb.position += velocity.normalized * SPEED * Time.deltaTime;

    }

    private void OnTargetDiedCallback()
    {
        target = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Cannonball Hit Enemy");
        }

        if (!isServer)
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Cannonball Hit Enemy");
            BaseEntity enemy = collision.gameObject.GetComponent<BaseEntity>();
            enemy.TakeDamage(10, ownerPlayer.gameObject);
            NetworkServer.Destroy(gameObject);
        }
    }


}
