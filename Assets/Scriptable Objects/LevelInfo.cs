using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "Scriptable Objects/Level Info")]
public class LevelInfo : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public string LevelName { get; private set; }
    [TextArea(3, 5)]
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public float TimeLimitMinutes { get; private set; }
    [field: SerializeField] public string LevelScene { get; private set; }
}
