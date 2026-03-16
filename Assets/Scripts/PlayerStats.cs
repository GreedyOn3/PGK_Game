using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float movementSpeed = 8.0f;
    public float attack = 5.0f;
    public float defense = 5.0f;

    public void ApplyUpgrade(Upgrade upgrade)
    {
        switch (upgrade.type)
        {
            case UpgradeType.MovementSpeed:
                movementSpeed += 1.0f;
                break;
            case UpgradeType.Attack:
                attack += 1.0f;
                break;
            case UpgradeType.Defense:
                defense += 1.0f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
