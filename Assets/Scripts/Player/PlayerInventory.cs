using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(PlayerReferences))]
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private WeaponId startingWeapon;
    [SerializeField] private GameObject wandPrefab;
    [SerializeField] private GameObject whipPrefab;

    [SerializeField] private int capacity = 6;
    private readonly List<Weapon> _weapons = new();
    [SerializeField] private Transform weaponsContainer;

    public event Action OnInventoryChange;

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
        var playerReferences = GetComponent<PlayerReferences>();
        switch (weaponId)
        {
            case WeaponId.Wand:
                var wand = Instantiate(wandPrefab, weaponsContainer).GetComponent<Weapons.Wand>();
                wand.player = playerReferences;
                _weapons.Add(wand);
                break;
            case WeaponId.Whip:
                var whip = Instantiate(whipPrefab, weaponsContainer).GetComponent<Weapons.Whip>();
                whip.player = playerReferences;
                _weapons.Add(whip);
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
