using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : BaseItemInfo
{
    [field: SerializeField] public Weapon Prefab { get; private set; }
    [field: SerializeField] public List<StatInfo> UsedStats { get; private set; }
}
