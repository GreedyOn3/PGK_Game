using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "Scriptable Objects/Level Info")]
public class LevelInfo : ScriptableObject
{
    public string levelName;
    public float timeLimitMinutes;
}
