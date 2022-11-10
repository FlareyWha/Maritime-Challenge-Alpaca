using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AirHockeyPaddle : NetworkBehaviour
{

    [SerializeField]
    private Transform BoundaryUp, BoundaryDown, BoundaryLeft, BoundaryRight;

    private Rigidbody2D rb = null;
    private SpriteRenderer spriteRenderer = null;

    private bool isHeld = false;
    private Vector2 offset = Vector2.zero;

    [SerializeField]
    private AudioSource puckAudioSource;


    public override void OnStartAuthority()
    {
        Debug.Log("Taken Control of Paddle");
    }

  

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
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
            Vector2 touchPos = InputManager.GetTouchPos();
            Vector2 worldTouchPos = Camera.main.ScreenToWorldPoint(touchPos);
            Vector2 paddlePos = ConstraintWithinBoundary(worldTouchPos) + offset;
            // Move Paddle
            MoveRigidbody(paddlePos);

        }
        else if (InputManager.InputActions.Main.Tap.WasPressedThisFrame()
            && SpriteHandler.IsWithinSprite(transform.position, spriteRenderer))
        {
            Debug.Log("Air Hockey Paddle Held");
            isHeld = true;

            offset = transform.position - Camera.main.ScreenToWorldPoint(InputManager.GetTouchPos());
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

    private Vector2 ConstraintWithinBoundary(Vector2 pos)
    {
        pos.x = Mathf.Clamp(pos.x, BoundaryLeft.position.x + SpriteHandler.GetSpriteRadius(spriteRenderer)
            , BoundaryRight.position.x - SpriteHandler.GetSpriteRadius(spriteRenderer));
        pos.y = Mathf.Clamp(pos.y, BoundaryDown.position.y + SpriteHandler.GetSpriteRadius(spriteRenderer)
          , BoundaryUp.position.y - SpriteHandler.GetSpriteRadius(spriteRenderer));

        return pos;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AirHockeyPuck"))
        {
            // Play Sound Effect - Pos is just transform.position
            puckAudioSource.Play();
            Debug.Log("Playing HIT");
        }
    }

}
