using UnityEngine;

[CreateAssetMenu(fileName = "StatUpgradeInfo", menuName = "Scriptable Objects/Stat Upgrade Info")]
public class StatUpgradeInfo : ScriptableObject
{
    public Sprite image;
    public string title;
    [TextArea(3, 5)]
    public string description;
    public StatUpgradeType type;
}

public enum StatUpgradeType
{
    MovementSpeed,
    Attack,
    Defense,
}
