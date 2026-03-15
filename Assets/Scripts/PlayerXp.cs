using System;
using UnityEngine;

public class PlayerXp : MonoBehaviour
{
    public float value = 0.0f;
    public float maxValue = 100.0f;

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
