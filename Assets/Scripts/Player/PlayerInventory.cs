using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerReferences), typeof(PlayerStats))]
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private WeaponInfo startingWeapon;
    [SerializeField] private SpecialItemInfo startingSpecialItem; // FOR TESTING

    [SerializeField] private int weaponCapacity = 6;
    [SerializeField] private int passivesCapacity = 6;
    private readonly List<Weapon> _weapons = new();
    private readonly List<PassiveItem> _passives = new();
    private readonly Dictionary<SpecialItemInfo, SpecialItem> _specials = new();
    [SerializeField] private Transform weaponsContainer;
    [SerializeField] private Transform specialItemsContainer;

    private PlayerStats _stats;
    public event Action OnInventoryChange;
    public event Action OnSpecialsChange;

    private void Start()
    {
        _stats = GetComponent<PlayerStats>();
        AddWeapon(startingWeapon);
        AddSpecial(startingSpecialItem);
    }

    public void AddWeapon(WeaponInfo weaponInfo)
    {
        var weaponPrefab = weaponInfo.Prefab;
        var weapon = Instantiate(weaponPrefab, weaponsContainer).GetComponent<Weapon>();
        _weapons.Add(weapon);
        LevelUpSystem.Instance.CacheWeaponUpgrades(weaponInfo);
        OnInventoryChange?.Invoke();
    }

    public void AddPassive(PassiveItemInfo passiveInfo)
    {
        PassiveItem passive = new PassiveItem() { info = passiveInfo, modifier = _stats.GetStatModifier(passiveInfo.Stat.Type) };
        //_stats.ApplyStatUpgrade(passiveInfo.StatUpgradeId, passive.percentage);
        _stats.IncreaseModifier(passiveInfo.Stat.Type, passiveInfo.BaseValue);
        _passives.Add(passive);
        OnInventoryChange?.Invoke();
    }

    public void AddSpecial(SpecialItemInfo specialItemInfo)
    {
        if(_specials.ContainsKey(specialItemInfo))
        {
            _specials[specialItemInfo].Count++;
        }
        else
        {
            GameObject prefab = specialItemInfo.ItemPrefab;
            SpecialItem special = Instantiate(prefab, specialItemsContainer).GetComponent<SpecialItem>();
            special.Init(GetComponent<PlayerReferences>());

            _specials.Add(specialItemInfo, special);
        }

        OnSpecialsChange?.Invoke();
    }

    public void UpgradePassive(PassiveItemInfo passiveInfo, float amount)
    {
        //_stats.ApplyStatUpgrade(passiveInfo.StatUpgradeId, 5f);
        
        _stats.IncreaseModifier(passiveInfo.Stat.Type, amount);
    }
    public void UpgradeWeapon(WeaponInfo weapon, List<StatInfo> stats)
    {
        Weapon foundWeapon = _weapons.Find(w => w.weaponInfo == weapon);
        if(foundWeapon != null)
        {
            foreach (StatInfo stat in stats)
                foundWeapon.AddModifier(stat.Type, stat.Value, stat.IsPercentage);
        }
    }

    public bool HasItem(BaseItemInfo item)
    {
        return _weapons.Exists(i => i.weaponInfo == item) || _passives.Exists(i => i.info == item);
    }

    public int GetWeaponCapacity() => weaponCapacity;
    public int GetPassivesCapacity() => passivesCapacity;

    public int GetWeaponCount() => _weapons.Count;
    public int GetPassivesCount() => _passives.Count;

    public bool IsFull() => (_weapons.Count + _passives.Count) == (weaponCapacity + passivesCapacity);

    public IReadOnlyList<Weapon> GetWeapons() => _weapons;
    public IReadOnlyList<PassiveItem> GetPassives() => _passives;
    public Dictionary<SpecialItemInfo, SpecialItem> GetSpecialItems() => _specials;
}
