using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int value = 100;
    [SerializeField] private int maxValue = 100;

    public int Value => value;
    public int MaxValue => maxValue;

    public void Add(int amount)
    {
        value = Math.Clamp(value + amount, 0, maxValue);
    }
}
