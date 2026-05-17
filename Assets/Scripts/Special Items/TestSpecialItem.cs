using UnityEngine;

public class TestSpecialItem : SpecialItem
{
    public int amount = 100;

    void Start()
    {
        _player.Stats.IncreaseModifier(StatType.Luck, amount);
    }
}
