using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{

    [SerializeField]
    private List<Animator> allPlayerAnimators;

    [SyncVar]
    private bool isWalking;
    [SyncVar]
    private Vector2 dir;

    private const float WALK_SPEED = 5.0f;

    private Rigidbody2D rb = null;

    public override void OnStartServer()
    {
        isWalking = false;
        dir = Vector2.zero;
    }

    public override void OnStartClient()
    {
        rb = GetComponent<Rigidbody2D>();
        foreach (Animator animator in allPlayerAnimators)
        {
            animator.SetBool("Walk", isWalking);
            animator.SetFloat("DirX", dir.x);
            animator.SetFloat("DirY", dir.y);
        }
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
        isWalking = walk;
        UpdateAnimatorWalkBool(walk);
    }

    [Command]
    private void SendUpdateAnimatorDir(float x, float y)
    {
        dir.Set(x, y);
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
