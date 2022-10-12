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

    public override void OnStartLocalPlayer()
    {
        foreach (Animator animator in allPlayerAnimators)
        {
            animator.Play(0);
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        Vector2 input = UIManager.Instance.Joystick.GetDirection();
        rb.position += input * WALK_SPEED * Time.deltaTime;

        SendUpdateAnimatorWalk(input.magnitude > 0);
        if (input.magnitude > 0)
        {
            SendUpdateAnimatorDir(input.x, input.y);
        }

    }

    [Command]
    private void SendUpdateAnimatorWalk(bool walk)
    {
        UpdateAnimatorWalkBool(walk);
    }

    [Command]
    private void SendUpdateAnimatorDir(float x, float y)
    {
        UpdateAnimatorDirValues(x, y);
    }

    [ClientRpc]
    private void UpdateAnimatorWalkBool(bool walk)
    {
        foreach (Animator animator in allPlayerAnimators)
        {
            animator.SetBool("Walk", walk);
        }
    }


    [ClientRpc]
    private void UpdateAnimatorDirValues(float x, float y)
    {
        foreach (Animator animator in allPlayerAnimators)
        {
            animator.SetFloat("DirX", x);
            animator.SetFloat("DirY", y);
        }
    }
}
