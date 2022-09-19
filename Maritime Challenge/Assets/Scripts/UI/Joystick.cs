using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    [SerializeField]
    private GameObject InnerCircle, OuterCircle;
    [SerializeField]
    private RectTransform FreeMoveArea;

    private float inner_radius = 0.0f;
    private float outer_radius = 0.0f;
    private float max_delta_radius = 0.0f;
    private float free_move_width = 0.0f;
    private float free_move_height = 0.0f;
    private bool isHeld = false;
    private Vector2 defaultPos;

    void Start()
    {
        defaultPos = OuterCircle.transform.position;

        inner_radius = InnerCircle.GetComponent<RectTransform>().rect.width * 0.5f;
        outer_radius = OuterCircle.GetComponent<RectTransform>().rect.width * 0.5f;
        max_delta_radius = inner_radius * 1.5f;

        free_move_height = FreeMoveArea.rect.height;
        free_move_width = FreeMoveArea.rect.width;

        if (!GameSettings.LOCK_JOYSTICK)
            HideJoystick();
    }

    void Update()
    {

        if (!isHeld && InputManager.InputActions.Main.Tap.WasPressedThisFrame()
             && IsWithinTouchArea())
        {
            Debug.Log("Joystick Held");
            isHeld = true;
            if (!GameSettings.LOCK_JOYSTICK)
            {
                ShowJoystick();
                Vector2 touchWorldPos = InputManager.InputActions.Main.TouchPosition.ReadValue<Vector2>();
                MoveJoystick(touchWorldPos);
                ConstraintWithinScreen();
            }
        }
        else if (isHeld)
        {
            if (InputManager.InputActions.Main.Tap.WasReleasedThisFrame())
            {
                Debug.Log("Joystick Released");
                isHeld = false;
                InnerCircle.transform.position = OuterCircle.transform.position;
                Debug.Log("iNNER POS:" + InnerCircle.transform.position + "  outer shit: " + OuterCircle.transform.position);
                if (!GameSettings.LOCK_JOYSTICK)
                    HideJoystick();
                return;
            }

            Vector2 touchWorldPos = InputManager.InputActions.Main.TouchPosition.ReadValue<Vector2>();

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
        if (dis.magnitude <= 0.3)
            return Vector2.zero;

        //  float perc = dis.magnitude / max_delta_radius;
        //  perc = Mathf.Clamp(perc, 0.0f, 1.0f);

      
        return dis.normalized; // * perc;
    }

    private void HideJoystick()
    {
        InnerCircle.SetActive(false);
        OuterCircle.SetActive(false);
    }

    private void ShowJoystick()
    {
        InnerCircle.SetActive(true);
        OuterCircle.SetActive(true);
        ResetJoystick();
    }

    private void MoveJoystick(Vector2 pos)
    {
        InnerCircle.transform.position = pos;
        OuterCircle.transform.position = pos;
    }

    public void ResetJoystick()
    {
        InnerCircle.transform.position = OuterCircle.transform.position;
    }

    public void OnJoystickSettingsChanged()
    {
        if (GameSettings.LOCK_JOYSTICK)
        {
            ShowJoystick();
            MoveJoystick(defaultPos);
        }
        else
            HideJoystick();
    }

    private bool IsWithinTouchArea()
    {
        if (UIManager.IsPointerOverUIElement())
            return false;


        float minX, minY, maxX, maxY;
        minX = minY = maxX = maxY = 0;
        if (GameSettings.LOCK_JOYSTICK)
        {
            Vector3 buttonPos = InnerCircle.transform.position;
            minX = buttonPos.x - inner_radius;
            maxX = buttonPos.x + inner_radius;
            minY = buttonPos.y - inner_radius;
            maxY = buttonPos.y + inner_radius;
        }
        else
        {
            minX = FreeMoveArea.position.x - free_move_width * 0.5f;
            maxX = FreeMoveArea.position.x + free_move_width * 0.5f;
            minY = FreeMoveArea.position.y - free_move_height * 0.5f;
            maxY = FreeMoveArea.position.y + free_move_height * 0.5f;
        }


        Vector2 touchPos = InputManager.InputActions.Main.TouchPosition.ReadValue<Vector2>();

        if (touchPos.x < maxX && touchPos.x > minX
            && touchPos.y > minY && touchPos.y < maxY)
        {
            return true;
        }

        return false;
    }

    private void ConstraintWithinScreen()
    {
        Vector2 pos;
        pos.x = Mathf.Clamp(OuterCircle.transform.position.x, FreeMoveArea.position.x - free_move_width * 0.5f + max_delta_radius + inner_radius,
            FreeMoveArea.position.x + (free_move_width * 0.5f) - max_delta_radius - inner_radius);
        pos.y = Mathf.Clamp(OuterCircle.transform.position.y, FreeMoveArea.position.y - free_move_height * 0.5f + max_delta_radius + inner_radius,
            FreeMoveArea.position.y + free_move_height * 0.5f - max_delta_radius - inner_radius);
        MoveJoystick(pos);
    }

}
