using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponPrefabs", menuName = "Scriptable Objects/Weapon Prefabs")]
public class WeaponPrefabs : ScriptableObject
{
    [field: SerializeField] public GameObject WandPrefab { get; private set; }
    [field: SerializeField] public GameObject WhipPrefab { get; private set; }

    /*public GameObject GetById(WeaponId id)
    {
        switch (id)
        {
            case WeaponId.Wand:
                return WandPrefab;
            case WeaponId.Whip:
                return WhipPrefab;
            default:
                throw new ArgumentOutOfRangeException(nameof(id), id, null);
        }
    }*/
}
