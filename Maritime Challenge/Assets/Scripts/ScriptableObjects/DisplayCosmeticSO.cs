using UnityEngine;

[CreateAssetMenu(fileName = "New Display Cosmetic", menuName = "Display Cosmetic")]
public class DisplayCosmeticSO : ScriptableObject
{
    public int CosmeticID;
    public BODY_PART_TYPE partType;
    public Sprite DisplaySprite;
}
