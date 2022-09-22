using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AirHockeyPaddle : NetworkBehaviour
{


    private bool isHeld = false;
    private Vector2 lastHeldPos = Vector2.zero;

    private Rigidbody2D rb = null;

    public override void OnStartAuthority()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("Taken Control of Paddle");
    }

    private void Update()
    {
        
        if (!hasAuthority)
            return;

        Debug.Log("HAS AUTHOROYTY");

        if (isHeld)
        {

            if (InputManager.InputActions.Main.Tap.WasReleasedThisFrame())
            {
                isHeld = false;
                return;
            }

            // Get Delta Pos
            Vector2 dis = InputManager.GetTouchPos() - lastHeldPos;
            Vector3 deltaPos = Camera.main.ScreenToWorldPoint(new Vector3(dis.x, dis.y, 1));
            // Move Paddle
            rb.position += new Vector2(deltaPos.x, deltaPos.y);

            lastHeldPos = InputManager.GetTouchPos();
        }
        else if ( IsWithinSprite() && InputManager.InputActions.Main.Tap.WasPressedThisFrame())// &&)
        {
            isHeld = true;
            lastHeldPos = InputManager.GetTouchPos();
        }
    }

    [Server]
    public void AssignController(NetworkIdentity player)
    { 
        this.netIdentity.AssignClientAuthority(player.connectionToClient);
    }

    [Server]
    public void RevokeControl()
    {
        netIdentity.RemoveClientAuthority();
    }

    protected bool IsWithinSprite()
    {
        // Get Player Sprite Size
        Vector2 spriteSize = GetSpriteSize();

        Vector2 touchPos = InputManager.GetTouchPos();
        Vector3 entityPos = UIManager.Instance.Camera.GetComponent<Camera>().WorldToScreenPoint(transform.position);
        if (touchPos.x < entityPos.x + spriteSize.x * 0.5f && touchPos.x > entityPos.x - spriteSize.x * 0.5f
            && touchPos.y > entityPos.y - spriteSize.y * 0.5f && touchPos.y < entityPos.y + spriteSize.y * 0.5f)
        {
            Debug.Log("Player Paddle Within Sprite");
            return true;
        }
        Debug.Log($"====Air Hockey Paddle IsWithinEntity()====\nTouch Pos: {touchPos} \nEntity Pos: {entityPos} \nSprite Size: {spriteSize}");
        return false;
    }

    public Vector2 GetSpriteSize()
    {
        Vector2 spriteSize = GetComponent<SpriteRenderer>().bounds.size * 0.5f;
        float pixelsPerUnit = GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        spriteSize.x *= pixelsPerUnit;
        spriteSize.y *= pixelsPerUnit;

        return spriteSize;
    }
}
