using UnityEngine;

public class WeaponInfo : BaseItemInfo
{
    [field: SerializeField] public Weapon WeaponPrefab { get; private set; }
    //[field: SerializeField] public WeaponId Id { get; private set; }
}

//public enum WeaponId
//{
//    Wand,
//    Whip,
//}
