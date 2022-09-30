using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Body Part", menuName = "Body Part")]
public class AvatarBodyPart : ScriptableObject
{
    // ~~ 1. Holds details about a body part's animations

    // Body Part Details
    public string bodyPartName;
    public int bodyPartID;

    // List Containing All Body Part Animations
    public List<AnimationClip> allBodyPartAnimations = new List<AnimationClip>();
}