using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerXp : MonoBehaviour
{
    [SerializeField] private int value = 0;
    [SerializeField] private int maxValue = 100;
    [SerializeField] private List<SpecialLevel> specialLevels = new();

    private SpecialLevel _currSpecial;

    public int Value => value;
    public int MaxValue => maxValue;

    public int Level { get; private set; } = 0;
    public event Action OnLevelUp;

    public void Add(int amount)
    {
        value += amount;

        while (value >= maxValue)
        {
            value -= maxValue;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;

        if (_currSpecial == null)
        {
            SpecialLevel special = specialLevels.Find(x => x.Level == Level);
            if (special != null)
            {
                _currSpecial = special;
                maxValue += special.Amount;
            }
        } 
        else
        {
            maxValue -= _currSpecial.Amount;
            _currSpecial = null;
        }

        if (Level <= 20)
            maxValue += 100;
        else if (Level <= 40)
            maxValue += 160;
        else
            maxValue += 220;

        OnLevelUp?.Invoke();
    }

    [Serializable]
    class SpecialLevel
    {
        public int Level;
        public int Amount;
    }
}
