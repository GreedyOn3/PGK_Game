using UnityEngine;

[CreateAssetMenu(fileName = "StatUpgradeInfo", menuName = "Scriptable Objects/Stat Upgrade Info")]
public class StatUpgradeInfo : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public string Title { get; private set; }
    [TextArea(3, 5)]
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public StatUpgradeId Id { get; private set; }
}

public enum StatUpgradeId
{
    MovementSpeed,
    Attack,
    Defense,
    PickupRange,
}
