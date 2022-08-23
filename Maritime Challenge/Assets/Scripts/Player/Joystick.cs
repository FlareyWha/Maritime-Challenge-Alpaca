using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public GameObject InnerCircle, OuterCircle;

    private float inner_radius = 0.0f;
    private const float max_delta_radius = 1.0f;
    private bool isHeld = false;

    void Start()
    {
        inner_radius = Camera.main.WorldToScreenPoint(InnerCircle.transform.lossyScale).x;
    }

    void Update()
    {
        if (!isHeld && InputManager.InputActions.Main.Tap.WasPressedThisFrame()
             && IsWithinButton())
        {
            isHeld = true;
            Debug.Log("Joystick Held");
        }
        else if (isHeld)
        {
            if (InputManager.InputActions.Main.Tap.WasReleasedThisFrame())
            {
                isHeld = false;
                InnerCircle.transform.position = OuterCircle.transform.position;
                Debug.Log("Joystick Released");
                return;
            }

            Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(InputManager.InputActions.Main.TouchPosition.ReadValue<Vector2>());

            Vector2 oriPos = new Vector2(OuterCircle.transform.position.x, OuterCircle.transform.position.y);
            if (Vector2.Distance(touchWorldPos, oriPos) > max_delta_radius)
            {
                touchWorldPos = oriPos + (touchWorldPos - oriPos).normalized * max_delta_radius;
            }
            InnerCircle.transform.position = touchWorldPos;

        }
    }
    
    public Vector2 GetDirection()
    {
        Vector2 dis = InnerCircle.transform.position - OuterCircle.transform.position;
        if (dis.magnitude == 0)
            return Vector2.zero;
        else return dis;
    }

    private bool IsWithinButton()
    {
        Vector2 touchPos = InputManager.InputActions.Main.TouchPosition.ReadValue<Vector2>();
        Vector3 buttonPos = Camera.main.WorldToScreenPoint(InnerCircle.transform.position);
   
        if (touchPos.x < buttonPos.x + inner_radius && touchPos.x > buttonPos.x - inner_radius
            && touchPos.y > buttonPos.y - inner_radius && touchPos.y < buttonPos.y + inner_radius)
        {
            return true;
        }

        return false;
    }

}
