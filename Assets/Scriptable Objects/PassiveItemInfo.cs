using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemInfo", menuName = "Scriptable Objects/Passive Item Info")]
public class PassiveItemInfo : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [TextArea(3, 5)]
    [field: SerializeField] public string Description { get; private set; }

    [field: SerializeField] public StatUpgradeId StatUpgradeId { get; private set; }
    [field: SerializeField] public float BasePercentage { get; private set; }
}

[System.Serializable]
public class PassiveItem
{
    public PassiveItemInfo info;
    public float percentage;
}