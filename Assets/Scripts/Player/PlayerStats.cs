using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Resources")]
    public int resourceGathered = 0;
    public float resourceDamage = 3f;

    [Header("Stats")]
    [SerializeField] private BasePlayerStats baseStats;
    [SerializeField] private CharacterStats characterStats;

    public PlayerStatsModifiers Modifiers { get; private set; }

    public float MovementSpeed => ApplyStatModifier(baseStats.MovementSpeed, Modifiers.MovementSpeedModifier);
    public float Attack => ApplyStatModifier(baseStats.Attack, Modifiers.AttackModifier);
    public float Defense => ApplyStatModifier(baseStats.Defense, Modifiers.DefenseModifier);
    public float PickupRange => ApplyStatModifier(baseStats.PickupRange, Modifiers.PickupRangeModifier);

    private void Awake()
    {
        Modifiers = new PlayerStatsModifiers
        {
            MovementSpeedModifier = characterStats.MovementSpeedModifier,
            AttackModifier = characterStats.AttackModifier,
            DefenseModifier = characterStats.DefenseModifier,
            PickupRangeModifier = characterStats.PickupRangeModifier,
        };
    }

    public void ApplyStatUpgrade(StatUpgradeId upgrade, float percentage)
    {
        var modifiers = Modifiers;

        switch (upgrade)
        {
            case StatUpgradeId.MovementSpeed:
                modifiers.MovementSpeedModifier += percentage;
                break;
            case StatUpgradeId.Attack:
                modifiers.AttackModifier += percentage;
                break;
            case StatUpgradeId.Defense:
                modifiers.DefenseModifier += percentage;
                break;
            case StatUpgradeId.PickupRange:
                modifiers.PickupRangeModifier += percentage;
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
    }
}

public struct PlayerStatsModifiers
{
    public float MovementSpeedModifier;
    public float AttackModifier;
    public float DefenseModifier;
    public float PickupRangeModifier;
}
