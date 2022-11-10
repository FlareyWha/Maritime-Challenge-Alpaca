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
    public void SendUpdateAnimatorAttack()
    {
        UpdateAnimatorWalkBool(false);
        UpdateAnimatorAttackTrigger();
    }

    [Server]
    public void SendUpdateAnimatorDie(bool die)
    {
        UpdateAnimatorDieBool(die);
        animator.SetBool("Die", die);
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
    private void UpdateAnimatorAttackTrigger()
    {
        animator.SetTrigger("Attack");
    }

    [ClientRpc]
    private void UpdateAnimatorDieBool(bool die)
    {
        animator.SetBool("Die", die);

        Debug.Log("Dieeeeeeeeeee");
    }


    [ClientRpc]
    private void UpdateAnimatorDirValues(float x)
    {
        animator.SetFloat("DirX", x);
    }
}
