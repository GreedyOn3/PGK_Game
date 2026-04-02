using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Resources")]
    public int resourceGathered = 0;
    public float resourceDamage = 3f;

    [Header("Stats")]
    [SerializeField] private float movementSpeed = 8.0f;
    [SerializeField] private float attack = 5.0f;
    [SerializeField] private float defense = 5.0f;
    [SerializeField] private float pickupRange = 5.0f;

    public float MovementSpeed => movementSpeed;
    public float Attack => attack;
    public float Defense => defense;
    public float PickupRange => pickupRange;

    public void ApplyStatUpgrade(StatUpgradeId upgrade)
    {
        switch (upgrade)
        {
            case StatUpgradeId.MovementSpeed:
                movementSpeed += 1.0f;
                break;
            case StatUpgradeId.Attack:
                attack += 1.0f;
                break;
            case StatUpgradeId.Defense:
                defense += 1.0f;
                break;
            case StatUpgradeId.PickupRange:
                pickupRange += 1.0f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
