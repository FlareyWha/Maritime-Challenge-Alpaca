using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CannonBall : BaseProjectile
{

   

    public override void Awake()
    {
        base.Awake();

        if (isClient)
            gameObject.SetActive(false);
    }

    [Server]
    public void Init(GameObject target, Player owner)
    {
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

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // Update Rotation
        if (isClient)
        {
            Vector3 rot = transform.rotation.eulerAngles;
            rot.z += 50.0f * Time.deltaTime;
            if (rot.z > 360)
                rot.z -= 360;
            transform.rotation = Quaternion.Euler(rot);
        }



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
            enemy.TakeDamage(ownerPlayer.ATK, ownerPlayer.gameObject);
            NetworkServer.Destroy(gameObject);
        }
    }


}
