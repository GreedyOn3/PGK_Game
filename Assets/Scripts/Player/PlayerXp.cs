using System;
using UnityEngine;

public class PlayerXp : MonoBehaviour
{
    [SerializeField] private int value = 0;
    [SerializeField] private int maxValue = 100;

    public int Value => value;
    public int MaxValue => maxValue;

    public int Level { get; private set; } = 0;
    public event Action OnLevelUp;

    public void Add(int amount)
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
