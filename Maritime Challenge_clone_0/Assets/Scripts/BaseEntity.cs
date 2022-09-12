using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public class BaseEntity : NetworkBehaviour
{
    [SerializeField]
    [SyncVar(hook = nameof(OnHPChanged))]
    protected int hp;

    [SerializeField]
    protected int maxHp;
    //protected int def;

    [SerializeField]
    protected int atk;

    [SerializeField]
    protected float atkspd;

    [SerializeField]
    protected float critrate;

    [SerializeField]
    protected float critdmg;

    [SerializeField]
    protected float movespd = 1;

    protected void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;

        //Call required stuff if entity dies 
        if (hp <= 0)
            HandleDeath();
    }

    protected virtual void HandleDeath()
    {

    }

    protected virtual void OnHPChanged(int oldHP, int newHP)
    {

    }
}
