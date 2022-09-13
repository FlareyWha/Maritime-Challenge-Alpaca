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


    protected void InitSpriteSize()
    {
        // Get Player Sprite Size
        spriteSize = GetComponent<SpriteRenderer>().bounds.size;
        float pixelsPerUnit = GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        spriteSize.x *= pixelsPerUnit;
        spriteSize.y *= pixelsPerUnit;
    }

    protected void CheckForEntityClick()
    {
        if (InputManager.InputActions.Main.Tap.WasPressedThisFrame() && IsWithinEntity())
        {
            gameObject.GetComponent<BaseEntity>().OnEntityClicked();
            Debug.Log("Invoking On Entity Clicked Event");
        }
    }
    public virtual void OnEntityClicked()
    {
        Debug.Log("Base Entity Clicked Called");
    }

    protected bool IsWithinEntity()
    {
        Vector2 touchPos = InputManager.InputActions.Main.TouchPosition.ReadValue<Vector2>();
        Vector3 playerPos = UIManager.Instance.Camera.GetComponent<Camera>().WorldToScreenPoint(transform.position);
        if (touchPos.x < playerPos.x + spriteSize.x * 0.5f && touchPos.x > playerPos.x - spriteSize.x * 0.5f
            && touchPos.y > playerPos.y - spriteSize.y * 0.5f && touchPos.y < playerPos.y + spriteSize.y * 0.5f)
        {
            return true;
        }

        return false;
    }
}
