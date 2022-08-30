using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInteract : NetworkBehaviour
{

    private PlayerUI playerUI = null;
    private Vector2 playerSize;

    private bool isInteractOpen = false;


    void Start()
    {
        playerUI = GetComponent<PlayerUI>();

        // Get Player Sprite Size
        playerSize = GetComponent<SpriteRenderer>().bounds.size;
        float pixelsPerUnit = GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        playerSize.x *= pixelsPerUnit;
        playerSize.y *= pixelsPerUnit;

        Debug.Log("PLayer Sixe: " + playerSize);
    }

    void Update()
    {
        if (isLocalPlayer)
            return;

        if (!isInteractOpen && InputManager.InputActions.Main.Tap.WasPressedThisFrame() && IsWithinPlayer())
        {
            isInteractOpen = true;
            playerUI.ShowInteractPanel();
        }
    }

    public void CloseInteractPanel()
    {
        isInteractOpen = false;
        playerUI.HideInteractPanel();
    }

    bool IsWithinPlayer()
    {
        Vector2 touchPos = InputManager.InputActions.Main.TouchPosition.ReadValue<Vector2>();
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        if (touchPos.x < playerPos.x + playerSize.x * 0.5f && touchPos.x > playerPos.x - playerSize.x * 0.5f
            && touchPos.y > playerPos.y - playerSize.y * 0.5f && touchPos.y < playerPos.y + playerSize.y * 0.5f)
        {
            return true;
        }

        //Debug.Log(transform.position);
        return false;
    }


}
