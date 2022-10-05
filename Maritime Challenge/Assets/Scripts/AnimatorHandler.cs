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

    private List<string> playerStatesList = new List<string>() { "idle" , "walk" };
    private List<string> playerDirectionsList = new List<string>() { "up", "down", "left", "right" };

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
                if (part.cosmetic.bodyPartType == CosmeticType.HAIR)
                {
                    UpdateAnimationClip(state, dir, partType, "front", partId);
                    UpdateAnimationClip(state, dir, partType, "back", partId);
                }
                else
                {
                    UpdateAnimationClip(state, dir, partType, "", partId);
                }
            }
        }
      
        // Apply updated animations
        animatorOverrideController.ApplyOverrides(defaultAnimationClips);
    }

    public void SetAnimations(CosmeticType type, int cosmeticID, string partTypeName)
    {
        foreach (string state in playerStatesList)
        {
            foreach (string dir in playerDirectionsList)
            {
                if (type == CosmeticType.HAIR)
                {
                    UpdateAnimationClip(state, dir, partTypeName, "front", cosmeticID.ToString());
                    UpdateAnimationClip(state, dir, partTypeName, "back", cosmeticID.ToString());
                }
                else
                {
                    UpdateAnimationClip(state, dir, partTypeName, "" , cosmeticID.ToString());
                }
            }
        }

        // Apply updated animations
        animatorOverrideController.ApplyOverrides(defaultAnimationClips);
    }

    private void UpdateAnimationClip(string state, string dir, string partTypeName, string detail, string cosmeticID)
    {
        animationClip = Resources.Load<AnimationClip>("PlayerAnimations/" + FileHeader + "/" + partTypeName + detail + "_" + cosmeticID + "_" + state + "_" + dir);
        // Override default animation
        defaultAnimationClips[partTypeName + detail + "_" + defaultID + "_" + state + "_" + dir] = animationClip;
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
