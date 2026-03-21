using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Weapons;

public class PlayerInventory : MonoBehaviour
{
    public WeaponId startingWeapon;
    public WeaponInfo wandWeaponInfo;
    public WeaponInfo whipWeaponInfo;
    [SerializeField] private int capacity = 6;

    public event Action OnInventoryChange;

    private List<Weapon> _weapons = new();

    private void Awake()
    {
        AddWeapon(startingWeapon);
    }

    private void Start()
    {
        OnInventoryChange?.Invoke();
    }

    public void AddWeapon(WeaponId weaponId)
    {
        Assert.IsTrue(_weapons.Count < capacity);
        switch (weaponId)
        {
            case WeaponId.Wand:
                _weapons.Add(new Wand(wandWeaponInfo));
                break;
            case WeaponId.Whip:
                _weapons.Add(new Whip(whipWeaponInfo));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(weaponId), weaponId, null);
        }
        OnInventoryChange?.Invoke();
    }

    public int GetCapacity()
    {
        return capacity;
    }

    public IReadOnlyList<Weapon> GetWeapons()
    {
        return _weapons;
    }
}
