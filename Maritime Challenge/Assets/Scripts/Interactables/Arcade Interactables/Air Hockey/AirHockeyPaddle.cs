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

    [Client]
    private void Awake()
    {
       // StartCoroutine(InitOnSpawn());
    }


    IEnumerator InitOnSpawn()
    {
        yield return null;
    }

    private void Update()
    {
       

        if (!hasAuthority)
            return;


        if (isHeld)
        {

            if (InputManager.InputActions.Main.Tap.WasReleasedThisFrame())
            {
                Debug.Log("Air Hockey Paddle Released");
                isHeld = false;
                return;
            }

            // Get Delta Pos
            Vector2 dis = InputManager.GetTouchPos() - lastHeldPos;
            Vector2 deltaPos = DisplayUtility.ConvertScreenToWorld(dis);
            Debug.Log("Delta Pos: " + deltaPos);
            // Move Paddle
            rb.position += new Vector2(deltaPos.x, deltaPos.y);

            lastHeldPos = InputManager.GetTouchPos();
        }
        else if (InputManager.InputActions.Main.Tap.WasPressedThisFrame()
            && SpriteHandler.IsWithinSprite(transform.position, GetComponent<SpriteRenderer>()))
        {
            Debug.Log("Air Hockey Paddle Held");
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

   

    public Vector2 GetSpriteSize()
    {
        Vector2 spriteSize = GetComponent<SpriteRenderer>().bounds.size * 0.5f;
        float pixelsPerUnit = GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        spriteSize.x *= pixelsPerUnit;
        spriteSize.y *= pixelsPerUnit;

        return spriteSize;
    }
}
