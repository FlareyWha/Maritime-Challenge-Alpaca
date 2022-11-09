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
    public void Init(GameObject target, Vector3 initialDir, Player owner)
    {
        this.target = target.GetComponent<BaseEnemy>();
        this.ownerPlayer = owner;
        velocity = initialDir * SPEED;// (target.transform.position - transform.position).normalized * SPEED;

        target.GetComponent<BaseEntity>().OnEntityDied += OnTargetDiedCallback;
        //Show();
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
        if (!isServer)
            return;

        if (target != null && collision.gameObject == this.target.gameObject)
        {
            BaseEntity enemy = collision.gameObject.GetComponent<BaseEntity>();
            enemy.TakeDamage(ownerPlayer.ATK, ownerPlayer.gameObject);
            SpawnHitVFX(transform.position);
            Deactivate();
        }
    }

    [ClientRpc]
    private void SpawnHitVFX(Vector3 pos)
    {
        if (PlayerData.activeSubScene != "WorldHubScene")
            return;
        VFXManager.Instance.AddVFX(VFX_TYPE.CANNONBALL_HIT, pos);
    }


}
