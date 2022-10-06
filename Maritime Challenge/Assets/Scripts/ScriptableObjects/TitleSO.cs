
using UnityEngine;

[CreateAssetMenu(fileName = "New Title", menuName = "Title")]
public class TitleSO : ScriptableObject
{
    public int ID;
    public string Name;
    public Sprite TitleSprite;
    public Sprite AchievementBackgroundSprite;
}
