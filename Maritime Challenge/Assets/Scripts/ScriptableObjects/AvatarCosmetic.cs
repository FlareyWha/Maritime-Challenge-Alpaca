using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cosmetic", menuName = "Cosmetic")]
public class AvatarCosmetic : ScriptableObject
{
    // ~~ 1. Holds details about a body part's animations

    // Body Part Details
    //public string bodyPartName;
    public int ID;
    public COSMETIC_TYPE BodyPartType;
    public Sprite IconSprite;

    // List Containing All Body Part Animations
    public List<AnimationClip> allBodyPartAnimations = new List<AnimationClip>();
}

public enum BODY_PART_TYPE
{
    HAIR_FRONT,
    HEADWEAR = 1,
    BODY = 2,
    TOP = 3,
    BOTTOM = 4,
    SHOE = 5,
    HAIR_BACK,

    NUM_TOTAL
}