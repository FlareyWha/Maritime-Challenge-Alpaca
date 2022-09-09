using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Battleship : NetworkBehaviour
{

    [SerializeField]
    private Sprite UpwardSprite, DownwardSprite, LeftSprite, RightSprite;

    private Rigidbody2D rb = null;
    private SpriteRenderer shipSprite = null;

    private Vector2 velocity = Vector2.zero;
    private Vector2 accel = Vector2.zero;
    private float accel_rate = 10.0f;
    private float deccel_rate = 5.0f;

    private const float MAX_VEL = 10.0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shipSprite = GetComponent<SpriteRenderer>();
    }

    public override void OnStartAuthority()
    {
        Debug.Log("BattleShip: Taken Authority Over BattleShip");
        PlayerData.MyPlayer.SetLinkedShip(this);
        gameObject.SetActive(false);
    }

   

    void Update()
    {
        if (!hasAuthority)
            return;

        // Update Ship Movement
        // Update Accel if Joystick is Moved
        if (UIManager.Instance.Joystick.GetDirection() != Vector2.zero)
        {
            accel = UIManager.Instance.Joystick.GetDirection();
            velocity += accel * accel_rate * Time.deltaTime;
        }
        // Stop Ship if coming to a stop  (curr dir diff from last accel dir) - so it doesn't go backwards from deccel
        if (velocity.magnitude != 0 && 
            Vector2.Dot(accel, velocity) < 0)
        {
            velocity = Vector2.zero;
        }

        // Apply Decceleration if ship is moving
        if (velocity.magnitude != 0)
            velocity -= velocity.normalized * deccel_rate * Time.deltaTime;

        // Clamp values wihtin Max Limit
        velocity.x = Mathf.Clamp(velocity.x, -MAX_VEL, MAX_VEL);
        velocity.y = Mathf.Clamp(velocity.y, -MAX_VEL, MAX_VEL);

        // Move Ship By Velocity
        rb.position += velocity * Time.deltaTime;

        // Update Ship Sprite
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            if (velocity.x > 0)
                shipSprite.sprite = RightSprite;
            else
                shipSprite.sprite = LeftSprite;
        }
        else
        {
            if (velocity.y > 0)
                shipSprite.sprite = UpwardSprite;
            else
                shipSprite.sprite = DownwardSprite;
        }

    }


    public void Dock()
    {
        gameObject.SetActive(false);
    }
    public void Summon(Transform refTransform)
    {
        gameObject.SetActive(true);
        transform.position = refTransform.position;
        transform.rotation = refTransform.rotation;
        shipSprite.sprite = LeftSprite;
    }
}

