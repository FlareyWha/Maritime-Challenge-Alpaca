using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorHandler : AnimatorHandler
{
    [SerializeField]
    protected string AnimPartName = "";

    private List<string> playerStatesList = new List<string>() { "idle" , "walk" };
    private List<string> playerDirectionsList = new List<string>() { "up", "down", "left", "right" };
    private string blankAnimFilePath = "PlayerAnimations/Blank";


    public override void SetAnimations(int cosmeticID)
    {
        foreach (string state in playerStatesList)
        {
            foreach (string dir in playerDirectionsList)
            {
                UpdateAnimationClip(state, dir, cosmeticID);
            }
        }

        // Apply updated animations
        animatorOverrideController.ApplyOverrides(defaultAnimationClips);
    }

    protected override void UpdateAnimationClip(string state, string dir, int id)
    {
        animationClip = Resources.Load<AnimationClip>("PlayerAnimations/" + FileHeader + "/" + AnimPartName + "_" + id.ToString() + "_" + state + "_" + dir);
        if (animationClip == null) // safe checkign
            animationClip = Resources.Load<AnimationClip>(blankAnimFilePath);

        defaultAnimationClips[AnimPartName + "_" + defaultID + "_" + state + "_" + dir] = animationClip;
    }

   
}
