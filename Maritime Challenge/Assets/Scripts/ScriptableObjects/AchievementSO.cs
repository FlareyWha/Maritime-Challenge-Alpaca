using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement")]
public class AchievementSO : ScriptableObject
{
    public int ID;
    public string Name;

    public PLAYER_STAT Type;
    public int Tier;
    public int RequirementNum;

    public Sprite BackgroundSprite;

    public TitleSO Title;
}
