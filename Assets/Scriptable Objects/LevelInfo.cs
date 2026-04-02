using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "Scriptable Objects/Level Info")]
public class LevelInfo : ScriptableObject
{
    public Sprite image;
    public string levelName;
    [TextArea(3, 5)]
    public string description;
    public float timeLimitMinutes;
    public LevelId id;
}

public enum LevelId
{
    Forest,
}
