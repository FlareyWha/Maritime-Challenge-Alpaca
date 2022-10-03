using UnityEngine;

[CreateAssetMenu(fileName = "New Avatar", menuName = "Avatar")]
public class AvatarSO : ScriptableObject
{
    // ~~ 1. Holds details about the full character body
    public AvatarPart[] avatarParts;
}

[System.Serializable]
public class AvatarPart
{
    public string bodyPartName;
    public AvatarCosmetic cosmetic;
}