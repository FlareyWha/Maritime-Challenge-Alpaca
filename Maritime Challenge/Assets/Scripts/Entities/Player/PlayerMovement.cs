using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{

    [SerializeField]
    private List<Animator> allPlayerAnimators;

    private const float WALK_SPEED = 5.0f;

    private Rigidbody2D rb = null;

    public override void OnStartClient()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        Vector2 input = UIManager.Instance.Joystick.GetDirection();
        rb.position += input * WALK_SPEED * Time.deltaTime;

        foreach (Animator animator in allPlayerAnimators)
        {
            animator.SetBool("Walk", input.magnitude > 0);
            animator.SetFloat("DirX", input.x);
            animator.SetFloat("DirY", input.y);
        }
    }

}
