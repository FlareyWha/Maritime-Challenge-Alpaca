using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    private Animator animator;
    private AnimationClip animationClip;
    private AnimatorOverrideController animatorOverrideController;
    private AnimationClipOverrides defaultAnimationClips;

    private List<string> playerStatesList = new List<string>() { "idle", "walk" };
    private List<string> playerDirectionsList = new List<string>() { "up", "down", "left", "right" };

    private void Start()
    {
        // Set animator
        animator = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        defaultAnimationClips = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(defaultAnimationClips);

    }

    public void SetAnimations(AvatarPart part)
    {
        string partId = part.cosmetic.bodyPartID.ToString();
        string partType = part.bodyPartName;
        foreach (string state in playerStatesList)
        {
            foreach (string dir in playerDirectionsList)
            {
                animationClip = Resources.Load<AnimationClip>("PlayerAnimations/" + partType + "/" + partType + "_" + partId + "_" + state + "_" + dir);

                // Override default animation
                defaultAnimationClips[partType + "_" + 0 + "_" + state + "_" + dir] = animationClip;
            }
        }
      
        // Apply updated animations
        animatorOverrideController.ApplyOverrides(defaultAnimationClips);
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
