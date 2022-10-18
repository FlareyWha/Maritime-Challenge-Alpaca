using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission")]
public class MissionSO : ScriptableObject
{
    public int ID;
    public string Name;

    public MISSION_TYPE Type;
    public int RequirementNum;
}
