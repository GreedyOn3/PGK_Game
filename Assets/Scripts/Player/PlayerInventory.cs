using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(PlayerReferences))]
public class PlayerInventory : MonoBehaviour
{
    //[SerializeField] private WeaponId startingWeapon;
    [SerializeField] private WeaponInfo startingWeapon;
    //[SerializeField] private WeaponPrefabs weaponPrefabs;

    [SerializeField] private int weaponCapacity = 6;
    [SerializeField] private int passivesCapacity = 6;
    private readonly List<Weapon> _weapons = new();
    private readonly List<PassiveItem> _passives = new();
    [SerializeField] private Transform weaponsContainer;

    public event Action OnInventoryChange;

    private void Start()
    {
        AddWeapon(startingWeapon);
    }

    public void AddWeapon(WeaponInfo weaponInfo)
    {
        //Assert.IsTrue(_weapons.Count < capacity);
        var weaponPrefab = weaponInfo.WeaponPrefab;
        var weapon = Instantiate(weaponPrefab, weaponsContainer).GetComponent<Weapon>();
        _weapons.Add(weapon);
        OnInventoryChange?.Invoke();
    }

    public void AddWeapon(PassiveItemInfo passiveInfo)
    {
        PassiveItem passive = new PassiveItem() { info = passiveInfo, percentage = passiveInfo.BasePercentage };
        _passives.Add(passive);
        OnInventoryChange?.Invoke();
    }

    public int GetWeaponCapacity() => weaponCapacity;
    public int GetPassivesCapacity() => passivesCapacity;

    public IReadOnlyList<Weapon> GetWeapons() => _weapons;
    public IReadOnlyList<PassiveItem> GetPassives() => _passives;
}
