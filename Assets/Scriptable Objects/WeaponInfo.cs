using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "Scriptable Objects/Weapon Info")]
public class WeaponInfo : ScriptableObject
{
    public Sprite image;
    public string title;
    [TextArea(3, 5)]
    public string description;
    public WeaponId id;
}

public enum WeaponId
{
    Wand,
    Whip,
}
