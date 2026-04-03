using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "Scriptable Objects/Weapon Info")]
public class WeaponInfo : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public string Title { get; private set; }
    [TextArea(3, 5)]
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public WeaponId Id { get; private set; }
}

public enum WeaponId
{
    Wand,
    Whip,
}
