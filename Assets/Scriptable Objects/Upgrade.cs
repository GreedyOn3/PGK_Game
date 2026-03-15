using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrade")]
public class Upgrade : ScriptableObject
{
    public string title;
    [TextArea(3, 5)]
    public string description;
    public UpgradeType type;
}

public enum UpgradeType
{
    MovementSpeed,
    Attack,
    Defense,
}
