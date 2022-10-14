using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    [SerializeField]
    private int defaultID = 0;
    [SerializeField]
    private string FileHeader = "";
    [SerializeField]
    private string AnimPartName = "";

    private Animator animator;
    private AnimationClip animationClip;
    private AnimatorOverrideController animatorOverrideController;
    private AnimationClipOverrides defaultAnimationClips;

    private List<string> playerStatesList = new List<string>() { "idle" , "walk" };
    private List<string> playerDirectionsList = new List<string>() { "up", "down", "left", "right" };
    private string blankAnimFilePath = "PlayerAnimations/Blank";

    private void Awake()
    {
        // Set animator
        animator = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        defaultAnimationClips = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(defaultAnimationClips);

    }

 

    public void SetAnimations(int cosmeticID)
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

    private void UpdateAnimationClip(string state, string dir, int cosmeticID)
    {
        if (cosmeticID == PlayerAvatarManager.NullRefNum)
        {
            animationClip = Resources.Load<AnimationClip>(blankAnimFilePath);
            return;
        }

        animationClip = Resources.Load<AnimationClip>("PlayerAnimations/" + FileHeader + "/" + AnimPartName + "_" + cosmeticID.ToString() + "_" + state + "_" + dir);
        if (animationClip == null) // safe checkign
            animationClip = Resources.Load<AnimationClip>(blankAnimFilePath);

        defaultAnimationClips[AnimPartName + "_" + defaultID + "_" + state + "_" + dir] = animationClip;
    }

    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int capacity) : base(capacity) { }

        public AnimationClip this[string name]
        {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set
            {
                int index = this.FindIndex(x => x.Key.name.Equals(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }
}
