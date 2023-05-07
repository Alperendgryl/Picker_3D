using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int PlatformCount;
    public Vector3[] Positions;
    public Quaternion[] Rotations;
}