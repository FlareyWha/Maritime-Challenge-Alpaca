using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyAnimationHandler : NetworkBehaviour
{
    [SerializeField]
    private Animator animator;

    [Server]
    public void SendUpdateAnimatorWalk(bool walk)
    {
        UpdateAnimatorWalkBool(walk);
    }

    [Server]
    public void SendUpdateAnimatorDie(bool die)
    {
        UpdateAnimatorDieBool(die);
    }

    [Server]
    public void SendUpdateAnimatorDir(float x)
    {
        UpdateAnimatorDirValues(x);
    }

    [ClientRpc]
    private void UpdateAnimatorWalkBool(bool walk)
    {
        animator.SetBool("Walk", walk);
    }

    [ClientRpc]
    private void UpdateAnimatorDieBool(bool die)
    {
        animator.SetBool("Die", die);
    }


    [ClientRpc]
    private void UpdateAnimatorDirValues(float x)
    {
        animator.SetFloat("DirX", x);
    }
}
