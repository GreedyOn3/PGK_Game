using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/Character Stats")]
public class CharacterStats : ScriptableObject
{
    [field: SerializeField] public int MovementSpeedModifier { get; private set; }
    [field: SerializeField] public int AttackModifier { get; private set; }
    [field: SerializeField] public int DefenseModifier { get; private set; }
    [field: SerializeField] public int PickupRangeModifier { get; private set; }
}
