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

    public float MovementSpeed => ApplyStatModifier(baseStats.MovementSpeed, Modifiers.movementSpeedModifier);
    public float Attack => ApplyStatModifier(baseStats.Attack, Modifiers.attackModifier);
    public float PickupRange => ApplyStatModifier(baseStats.PickupRange, Modifiers.pickupRangeModifier);

    private void Awake()
    {
        Modifiers = new PlayerStatsModifiers
        {
            movementSpeedModifier = characterStats.MovementSpeedModifier,
            attackModifier = characterStats.AttackModifier,
            defenseModifier = characterStats.DefenseModifier,
            pickupRangeModifier = characterStats.PickupRangeModifier,
        };

        var persistentData = PersistentData.Instance;
        foreach (var permanentUpgrade in persistentData.permanentUpgrades)
        {
            if (permanentUpgrade.bought)
                ApplyPermanentStatUpgrade(permanentUpgrade);
        }
    }

    public void ApplyStatUpgrade(PlayerStatType statType, float percentage)
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

    private void ApplyPermanentStatUpgrade(PermanentUpgradeInfo upgrade)
    {
        ApplyStatUpgrade(upgrade.StatType, upgrade.IncreasePercentage);
    }

    private static float ApplyStatModifier(float baseValue, float modifier)
    {
        var multiplier = 1.0f + modifier / 100.0f;
        return baseValue * multiplier;
    }
}

public struct PlayerStatsModifiers
{
    public float movementSpeedModifier;
    public float attackModifier;
    public float defenseModifier;
    public float pickupRangeModifier;
}

public enum PlayerStatType
{
    MovementSpeed,
    Attack,
    Defense,
    PickupRange,
}
