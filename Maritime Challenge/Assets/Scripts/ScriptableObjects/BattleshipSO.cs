using UnityEngine;

[CreateAssetMenu(fileName = "New Battleship", menuName = "Battleship")]
public class BattleshipSO : ScriptableObject
{
    public int ID;
    public string Name;
    // Stats
    public Sprite[] DirectionSprites;
}
