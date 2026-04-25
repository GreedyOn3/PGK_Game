using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemInfo", menuName = "Scriptable Objects/Passive Item Info")]
public class PassiveItemInfo : BaseItemInfo
{
    //[field: SerializeField] public StatType StatType { get; private set; }
    [Header("Stat Info")]
    public StatInfo Stat;
    public float BaseValue;
}

[System.Serializable]
public class PassiveItem
{
    public PassiveItemInfo info;
    public StatModifier modifier;
}