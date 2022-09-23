using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPaddle : MonoBehaviour
{

    private Rigidbody2D rb = null;

    private bool isHeld = false;
    private Vector2 lastHeldPos = Vector2.zero;

 

    private Vector2 assumedVel = Vector2.zero;
    private Vector3 lastPosition = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    private void Update()
    {
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

    private void FixedUpdate()
    {
        assumedVel = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

    }



    public Vector2 GetVelocity()
    {
        return assumedVel;
    }

   
}
