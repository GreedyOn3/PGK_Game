using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "Scriptable Objects/Weapon Info")]
public class WeaponInfo : ScriptableObject
{
    [Header("Data")]
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [TextArea(3, 5)]
    [field: SerializeField] public string Description { get; private set; }
    [Header("Runtime")]
    [field: SerializeField] public Weapon WeaponPrefab { get; private set; }
    //[field: SerializeField] public WeaponId Id { get; private set; }
}

//public enum WeaponId
//{
//    Wand,
//    Whip,
//}
