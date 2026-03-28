using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 8.0f;
    [SerializeField] private float attack = 5.0f;
    [SerializeField] private float defense = 5.0f;

    public float MovementSpeed => movementSpeed;
    public float Attack => attack;
    public float Defense => defense;

    public void ApplyStatUpgrade(StatUpgradeType type)
    {
        switch (type)
        {
            case StatUpgradeType.MovementSpeed:
                movementSpeed += 1.0f;
                break;
            case StatUpgradeType.Attack:
                attack += 1.0f;
                break;
            case StatUpgradeType.Defense:
                defense += 1.0f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
