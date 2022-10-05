using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    [SerializeField]
    private int defaultID = 0;
    [SerializeField]
    private string FileHeader = "";

    private Animator animator;
    private AnimationClip animationClip;
    private AnimatorOverrideController animatorOverrideController;
    private AnimationClipOverrides defaultAnimationClips;

    private List<string> playerStatesList = new List<string>() { "idle" };//, "walk" };
    private List<string> playerDirectionsList = new List<string>() { "down", };//"up", "down", "left", "right" };

    private void Awake()
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
        if (part.cosmetic == null)
            return;

        string partId = part.cosmetic.ID.ToString();
        string partType = part.bodyPartName;
        foreach (string state in playerStatesList)
        {
            foreach (string dir in playerDirectionsList)
            {
                animationClip = Resources.Load<AnimationClip>("PlayerAnimations/" + FileHeader + "/" + partType + "_" + partId + "_" + state + "_" + dir);
        
                // TEMP
                if (defaultID == 0)
                {
                    return;
                }

                // Override default animation
                defaultAnimationClips[partType + "_" + defaultID + "_" + state + "_" + dir] = animationClip;
            }
        }
      
        // Apply updated animations
        animatorOverrideController.ApplyOverrides(defaultAnimationClips);
    }

    public void SetAnimations(int cosmeticID, string partTypeName)
    {
        foreach (string state in playerStatesList)
        {
            foreach (string dir in playerDirectionsList)
            {
                animationClip = Resources.Load<AnimationClip>("PlayerAnimations/" + FileHeader + "/" + partTypeName + "_" + cosmeticID + "_" + state + "_" + dir);

                // TEMP
                if (defaultID == 0)
                {
                    return;
                }

                // Override default animation
                defaultAnimationClips[partTypeName + "_" + defaultID + "_" + state + "_" + dir] = animationClip;
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
