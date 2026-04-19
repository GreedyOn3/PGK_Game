using UnityEngine;

[CreateAssetMenu(fileName = "PermanentUpgradeInfo", menuName = "Scriptable Objects/PermanentUpgradeInfo")]
public class PermanentUpgradeInfo : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public string UpgradeName { get; private set; }
    [TextArea(3, 5)]
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public StatType StatType { get; private set; }
    [field: SerializeField] public float IncreasePercentage { get; private set; }
    public bool bought;
}

[System.Serializable]
public class PermanentUpgradeEntry
{
    public string key;
    public bool value;

    public PermanentUpgradeEntry(string k, bool v) { key = k; value = v; }
}
