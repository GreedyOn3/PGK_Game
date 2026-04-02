using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponPrefabs", menuName = "Scriptable Objects/Weapon Prefabs")]
public class WeaponPrefabs : ScriptableObject
{
    public GameObject wandPrefab;
    public GameObject whipPrefab;

    public GameObject GetById(WeaponId id)
    {
        switch (id)
        {
            case WeaponId.Wand:
                return wandPrefab;
            case WeaponId.Whip:
                return whipPrefab;
            default:
                throw new ArgumentOutOfRangeException(nameof(id), id, null);
        }
    }
}
