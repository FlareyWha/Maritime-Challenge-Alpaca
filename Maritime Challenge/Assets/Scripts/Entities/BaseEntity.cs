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

    private Vector2 spriteSize;

    public delegate void EntityDied();
    public event EntityDied OnEntityDied;

    protected delegate void EntityHPChanged(int oldHP, int newHP);
    protected event EntityHPChanged OnEntityHPChanged;

    [Server]
    public void TakeDamage(int damageAmount, GameObject attacker)
    {
        hp -= damageAmount;
        Debug.Log("Take Damage Called, " + attacker.name + " dealt " + damageAmount);
        //Call required stuff if entity dies 
        if (hp <= 0)
        {
            InvokeOnEntityDied();
            HandleDeath();
        }

        OnDamageDealtOrTaken(damageAmount, false, attacker);
    }

    [ClientRpc]
    private void OnDamageDealtOrTaken(int hp_amt, bool dealt, GameObject attacker) 
    {
        Debug.Log("OnDamageDealtOrTakenCalled Callback from Rpc Called");
        if (attacker == PlayerData.MyPlayer.gameObject || this.gameObject == PlayerData.MyPlayer.gameObject)
            PopUpManager.Instance.AddHPChangeText(hp_amt, !dealt, this.transform);
    }


    [ClientRpc]
    private void InvokeOnEntityDied()
    {
        OnEntityDied?.Invoke();
    }
    protected virtual void HandleDeath()
    {
    }

    private void OnHPChanged(int oldHP, int newHP)
    {
        OnEntityHPChanged?.Invoke(oldHP, newHP);
    }

    protected void InitSpriteSize()
    {
        // Get Player Sprite Size
        spriteSize = GetComponent<SpriteRenderer>().bounds.size * 0.5f;
        float pixelsPerUnit = GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        spriteSize.x *= pixelsPerUnit;
        spriteSize.y *= pixelsPerUnit;
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

    protected bool IsWithinEntity()
    {
        Vector2 touchPos = InputManager.InputActions.Main.TouchPosition.ReadValue<Vector2>();
        Vector3 entityPos = UIManager.Instance.Camera.GetComponent<Camera>().WorldToScreenPoint(transform.position);
        if (touchPos.x < entityPos.x + spriteSize.x * 0.5f && touchPos.x > entityPos.x - spriteSize.x * 0.5f
            && touchPos.y > entityPos.y - spriteSize.y * 0.5f && touchPos.y < entityPos.y + spriteSize.y * 0.5f)
        {
            return true;
        }

        return false;
    }

    public Vector2 GetSpriteSize()
    {
        return spriteSize;
    }

    public float GetSpriteRadius()
    {
        return (spriteSize.x + spriteSize.y) * 0.5f;
    }

    public float GetSpriteSizeMax()
    {
        return spriteSize.x > spriteSize.y ? spriteSize.x : spriteSize.y;
    }

    public int GetMaxHP()
    {
        return maxHp;
    }
}
