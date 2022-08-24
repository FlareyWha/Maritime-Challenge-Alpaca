using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField]
    private Text PlayerDisplayName;
    private int PlayerID;

    private const float WALK_SPEED = 3.0f;

    public override void OnStartLocalPlayer()
    {
        // Init from Player Data
        PlayerDisplayName.text = PlayerData.Name;
        PlayerID = PlayerData.ID;

        // Init Player to SpawnPos
        
        // Attach Camera
        UIManager.Instance.Camera.SetFollowTarget(gameObject);

    }

  
    void Update()
    {
        if (!isLocalPlayer)
            return;

        Vector2 input = UIManager.Instance.Joystick.GetDirection();
        transform.position += new Vector3(input.x, input.y, 0.0f) * WALK_SPEED * Time.deltaTime;
        
    }
}
