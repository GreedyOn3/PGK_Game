using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Resources")]
    public int resourceGathered = 0;
    public float resourceDamage = 3f;

    [Header("Stats")]
    [SerializeField] private BasePlayerStats baseStats;
    [SerializeField] private CharacterStats characterStats;

    private Dictionary<StatType, Stat> _stats = new Dictionary<StatType, Stat>();
    private Dictionary<StatType, StatModifier> _modifiers = new Dictionary<StatType, StatModifier>();

    private void Awake()
    {
        foreach (StatInfo statInfo in baseStats.BaseStats)
            _stats.Add(statInfo.Type, new Stat(statInfo.Value));

        foreach (StatInfo statInfo in characterStats.Stats)
        {
            StatModifier mod = new StatModifier(statInfo.Value, statInfo.IsPercentage);
            _modifiers.Add(statInfo.Type, mod);

            if(_stats.ContainsKey(statInfo.Type))
                _stats[statInfo.Type].AddModifier(mod);
        }

        var persistentData = PersistentData.Instance;
        foreach (var permanentUpgrade in persistentData.permanentUpgrades)
        {
            if (permanentUpgrade.bought)
                ApplyPermanentStatUpgrade(permanentUpgrade);
        }
    }

    /*public void ApplyStatUpgrade(StatUpgradeId upgrade, float percentage)
    {
        var modifiers = Modifiers;

        switch (statType)
        {
            case PlayerStatType.MovementSpeed:
                modifiers.movementSpeedModifier += percentage;
                break;
            case PlayerStatType.Attack:
                modifiers.attackModifier += percentage;
                break;
            case PlayerStatType.Defense:
                modifiers.defenseModifier += percentage;
                break;
            case PlayerStatType.PickupRange:
                modifiers.pickupRangeModifier += percentage;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Modifiers = modifiers;
    }

    private static float ApplyStatModifier(float baseValue, float modifier)
    {
        var multiplier = 1.0f + modifier / 100.0f;
        return baseValue * multiplier;
    }*/

    private void ApplyPermanentStatUpgrade(PermanentUpgradeInfo upgrade)
    {
        //ApplyStatUpgrade(upgrade.StatType, upgrade.IncreasePercentage);
        IncreaseModifier(upgrade.StatType, upgrade.IncreasePercentage);
    }

    public float CalculateWeaponStat(StatType type, Weapon weapon)
    {
        if (!_modifiers.TryGetValue(type, out StatModifier mod)) return 0f;

        if (mod.isPercentage)
            return weapon.GetStatValue(type) * (1f + mod.value / 100f);
        else
            return weapon.GetStatValue(type) + mod.value;
    }

    public float GetStatValue(StatType type)
    {
        if (_stats.TryGetValue(type, out Stat stat))
            return stat.GetValue();

        return 0f;
    }

    public StatModifier GetStatModifier(StatType type)
    {
        if (_modifiers.TryGetValue(type, out StatModifier mod))
            return mod;

        return null;
    }
    public float GetModifierValue(StatType type)
    {
        if (_modifiers.TryGetValue(type, out StatModifier mod))
            return mod.value;

        return 0f;
    }

    public void IncreaseModifier(StatType type, float amount)
    {
        if (!_modifiers.TryGetValue(type, out StatModifier mod)) return;
        mod.value += amount;
    }
}

