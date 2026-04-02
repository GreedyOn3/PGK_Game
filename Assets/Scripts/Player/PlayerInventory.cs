using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(PlayerReferences))]
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private WeaponId startingWeapon;
    [SerializeField] private WeaponPrefabs weaponPrefabs;

    [SerializeField] private int capacity = 6;
    private readonly List<Weapon> _weapons = new();
    [SerializeField] private Transform weaponsContainer;

    public event Action OnInventoryChange;

    private void Start()
    {
        AddWeapon(startingWeapon);
    }

    public void AddWeapon(WeaponId weaponId)
    {
        Assert.IsTrue(_weapons.Count < capacity);
        var weaponPrefab = weaponPrefabs.GetById(weaponId);
        var weapon = Instantiate(weaponPrefab, weaponsContainer).GetComponent<Weapon>();
        _weapons.Add(weapon);
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
