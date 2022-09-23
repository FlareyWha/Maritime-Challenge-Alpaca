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

    public int HP
    {
        get { return hp; }
        private set { }
    }
    public int MaxHP
    {
        get { return maxHp; }
        private set { }
    }
    public int ATK
    {
        get { return atk; }
        private set { }
    }
    public float ATKSPD
    {
        get { return atkspd; }
        private set { }
    }


    public delegate void EntityDied();
    public event EntityDied OnEntityDied;

    protected delegate void EntityHPChanged(int oldHP, int newHP);
    protected event EntityHPChanged OnEntityHPChanged;

    protected GameObject killer;
    public GameObject Killer
    {
        get { return killer; }
        set { killer = value; }
    }

    [Server]
    public void TakeDamage(int damageAmount, GameObject attacker)
    {
        hp -= damageAmount;
        Debug.Log("Take Damage Called, " + attacker.name + " dealt " + damageAmount);
        //Call required stuff if entity dies 
        if (hp <= 0)
        {
            InvokeOnEntityDied();
            HandleDeath(attacker);
        }

        OnDamageDealtOrTaken(damageAmount, false, attacker);
    }

    [ClientRpc]
    private void OnDamageDealtOrTaken(int hp_amt, bool dealt, GameObject attacker)
    {
        Debug.Log("OnDamageDealtOrTakenCalled Callback from Rpc Called");
        if (attacker == PlayerData.MyPlayer.gameObject)
            PopUpManager.Instance.AddHPChangeText(hp_amt, !dealt, this.transform);
        else if (this.gameObject == PlayerData.MyPlayer.gameObject)
            PopUpManager.Instance.AddHPChangeText(hp_amt, !dealt, gameObject.GetComponent<Player>().GetBattleShip().transform);
    }


    [ClientRpc]
    private void InvokeOnEntityDied()
    {
        OnEntityDied?.Invoke();
    }

    protected virtual void HandleDeath(GameObject attacker)
    {
        killer = attacker;
    }

    private void OnHPChanged(int oldHP, int newHP)
    {
        OnEntityHPChanged?.Invoke(oldHP, newHP);
    }


    protected void CheckForEntityClick()
    {
        if (InputManager.InputActions.Main.Tap.WasPressedThisFrame() && IsWithinEntity())
        {
            gameObject.GetComponent<BaseEntity>().OnEntityClicked();
        }
    }
    public virtual void OnEntityClicked()
    {
    }

    public bool IsWithinEntity()
    {
        return SpriteHandler.IsWithinSprite(transform.position, GetComponent<SpriteRenderer>());
    }

   

    public float GetSpriteRadius()
    {
        Vector2 spriteSize = SpriteHandler.GetSpriteSize(GetComponent<SpriteRenderer>());
        return (spriteSize.x + spriteSize.y) * 0.25f;
    }

    public float GetSpriteSizeMax()
    {
        Vector2 spriteSize = SpriteHandler.GetSpriteSize(GetComponent<SpriteRenderer>());
        return spriteSize.x > spriteSize.y ? spriteSize.x : spriteSize.y;
    }

}
