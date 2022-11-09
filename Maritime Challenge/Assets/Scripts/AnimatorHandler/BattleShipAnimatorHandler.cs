using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleShipAnimatorHandler : AnimatorHandler
{
    private List<string> statesList = new List<string>() { "idle", "moving" };
    private List<string> directionsList = new List<string>() { "up", "down", "left", "right" };
    private string blankAnimFilePath = "PlayerAnimations/Blank";
    public override void SetAnimations(int shipID)
    {
        foreach (string state in statesList)
        {
            foreach (string dir in directionsList)
            {
                UpdateAnimationClip(state, dir, shipID);
            }
        }

        // Apply updated animations
        animatorOverrideController.ApplyOverrides(defaultAnimationClips);
    }
    protected override void UpdateAnimationClip(string state, string dir, int id)
    {
        animationClip = Resources.Load<AnimationClip>("BoatAnimations/" + id.ToString() + "_" + state + "_" + dir);
        if (animationClip == null) // safe checkign
            animationClip = Resources.Load<AnimationClip>(blankAnimFilePath);

        defaultAnimationClips[defaultID.ToString() + "_" + state + "_" + dir] = animationClip;
    }

 
}
