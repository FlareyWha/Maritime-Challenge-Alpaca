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
    }

    private void FixedUpdate()
    {
        if (!hasAuthority)
            return;

        if (isHeld)
        {
            // Get Delta Pos
            Vector2 dis = InputManager.GetTouchPos() - lastHeldPos;
            Vector3 deltaPos = Camera.main.ScreenToWorldPoint(new Vector3(dis.x, dis.y, 1));
            // Move Paddle
            rb.position += new Vector2(deltaPos.x, deltaPos.y);

            lastHeldPos = InputManager.GetTouchPos();
        }
        else if (InputManager.InputActions.Main.Tap.WasPressedThisFrame() && IsWithinSprite())
        {
            isHeld = true;
            lastHeldPos = InputManager.GetTouchPos();
        }
    }

    [Server]
    public void AssignController(NetworkConnectionToClient conn)
    {
        netIdentity.AssignClientAuthority(conn);
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
        Vector2 spriteSize = GetComponent<SpriteRenderer>().bounds.size * 0.5f;
        float pixelsPerUnit = GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        spriteSize.x *= pixelsPerUnit;
        spriteSize.y *= pixelsPerUnit;

        return spriteSize;
    }
}
