using UnityEngine;

public class WeaponInfo : BaseItemInfo
{
    [field: SerializeField] public Weapon Prefab { get; private set; }
}
