using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemInfo", menuName = "Scriptable Objects/Passive Item Info")]
public class PassiveItemInfo : BaseItemInfo
{
    [field: SerializeField] public StatUpgradeId StatUpgradeId { get; private set; }
    [field: SerializeField] public float BasePercentage { get; private set; }
}

[System.Serializable]
public class PassiveItem
{
    public PassiveItemInfo info;
    public float percentage;
}