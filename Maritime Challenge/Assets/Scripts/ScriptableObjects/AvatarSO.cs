using UnityEngine;

[CreateAssetMenu(fileName = "New Avatar", menuName = "Avatar")]
public class AvatarSO : ScriptableObject
{
    public AvatarPart[] avatarParts;
}

[System.Serializable]
public class AvatarPart
{
    public string partName; // for my own ref
    public AvatarCosmetic cosmetic;
}