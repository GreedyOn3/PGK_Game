using UnityEngine;

public class TestSpecialItem : SpecialItem
{
    public int amount = 100;

    public override void OnAdd()
    {
        _player.Stats.IncreaseModifier(StatType.Luck, amount);
    }
}
