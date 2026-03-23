using System;
using UnityEngine;

public class PlayerXp : MonoBehaviour
{
    [SerializeField] private float value = 0.0f;
    [SerializeField] private float maxValue = 100.0f;

    public float Value => value;
    public float MaxValue => maxValue;

    public int Level { get; private set; } = 0;
    public event Action OnLevelUp;

    public void Add(float amount)
    {
        value += amount;

        while (value >= maxValue)
        {
            Level++;
            value -= maxValue;
            OnLevelUp?.Invoke();
        }
    }
}
