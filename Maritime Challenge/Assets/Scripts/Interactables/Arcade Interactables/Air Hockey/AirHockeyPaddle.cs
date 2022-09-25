using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AirHockeyPaddle : NetworkBehaviour
{


    private Rigidbody2D rb = null;

    private bool isHeld = false;
    private Vector2 lastHeldPos = Vector2.zero;


    public override void OnStartAuthority()
    {
        Debug.Log("Taken Control of Paddle");
    }

  

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }



    private void FixedUpdate()
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
            // Move Paddle
            MoveRigidbody(new Vector2(transform.position.x, transform.position.y) + deltaPos);

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

    [Command]
    private void MoveRigidbody(Vector2 newPos)
    {
        rb.MovePosition(newPos);
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


}
