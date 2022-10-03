using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cosmetic", menuName = "Cosmetic")]
public class AvatarCosmetic : ScriptableObject
{
    // ~~ 1. Holds details about a body part's animations

    // Body Part Details
    //public string bodyPartName;
    public int ID;
    public CosmeticType bodyPartType;
    public Sprite IconSprite;

    // List Containing All Body Part Animations
    public List<AnimationClip> allBodyPartAnimations = new List<AnimationClip>();
}

public enum CosmeticType
{
    HAIR,
    HEADWEAR,
    BODY,
    TOP,
    BOTTOM,
    SHOE,

    NUM_TOTAL = 6
}