using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public class BaseEntity : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHPChanged))]
    protected int hp;
    protected int maxHp;
    //protected int def;
    protected int atk;
    protected int atkspd;
    protected int critrate;
    protected int critdmg;
    protected int movespd;

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
