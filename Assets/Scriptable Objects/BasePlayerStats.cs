using UnityEngine;

[CreateAssetMenu(fileName = "BasePlayerStats", menuName = "Scriptable Objects/Base Player Stats")]
public class BasePlayerStats : ScriptableObject
{
    [field: SerializeField] public float MovementSpeed { get; private set; } = 8.0f;
    [field: SerializeField] public float Attack { get; private set; } = 5.0f;
    [field: SerializeField] public float Defense { get; private set; } = 5.0f;
    [field: SerializeField] public float PickupRange { get; private set; } = 5.0f;
}
