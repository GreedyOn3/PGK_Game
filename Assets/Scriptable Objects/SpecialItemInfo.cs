using UnityEngine;

[CreateAssetMenu(fileName = "SpecialItemInfo", menuName = "Scriptable Objects/Special Item Info")]
public class SpecialItemInfo : BaseItemInfo
{
    [field: SerializeField] public GameObject ItemPrefab { get; private set; }
}
