using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerReferences), typeof(PlayerStats))]
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private WeaponInfo startingWeapon;

    [SerializeField] private int weaponCapacity = 6;
    [SerializeField] private int passivesCapacity = 6;
    private readonly List<Weapon> _weapons = new();
    private readonly List<PassiveItem> _passives = new();
    [SerializeField] private Transform weaponsContainer;

    private PlayerStats _stats;
    public event Action OnInventoryChange;

    private void Start()
    {
        _stats = GetComponent<PlayerStats>();
        AddWeapon(startingWeapon);
    }

    public void AddWeapon(WeaponInfo weaponInfo)
    {
        var weaponPrefab = weaponInfo.Prefab;
        var weapon = Instantiate(weaponPrefab, weaponsContainer).GetComponent<Weapon>();
        _weapons.Add(weapon);
        OnInventoryChange?.Invoke();
    }

    public void AddPassive(PassiveItemInfo passiveInfo)
    {
        PassiveItem passive = new PassiveItem() { info = passiveInfo, percentage = passiveInfo.BasePercentage };
        _stats.ApplyStatUpgrade(passiveInfo.StatType, passive.percentage);
        _passives.Add(passive);
        OnInventoryChange?.Invoke();
    }

    public void UpgradePassive(PassiveItemInfo passiveInfo)
    {
        _stats.ApplyStatUpgrade(passiveInfo.StatType, 5f);
    }

    public bool HasItem(BaseItemInfo item)
    {
        return _weapons.Exists(i => i.weaponInfo == item) || _passives.Exists(i => i.info == item);
    }

    public int GetWeaponCapacity() => weaponCapacity;
    public int GetPassivesCapacity() => passivesCapacity;

    public int GetWeaponCount() => _weapons.Count;
    public int GetPassivesCount() => _passives.Count;

    public IReadOnlyList<Weapon> GetWeapons() => _weapons;
    public IReadOnlyList<PassiveItem> GetPassives() => _passives;
}
