using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Weapon : Armament
{
    public WeaponInfo weaponInfo;
    //[SerializeField] protected float cooldownSeconds = 5.0f;
    private Dictionary<StatType, Stat> _stats = new Dictionary<StatType, Stat>();

    public abstract void Attack();

    private void Start()
    {
        foreach (StatInfo statInfo in weaponInfo.UsedStats)
            _stats.Add(statInfo.Type, new Stat(statInfo.BaseValue));

        Invoke(nameof(PerformAttack), player.Stats.CalculateWeaponStat(StatType.Cooldown, this));
    }

    public void DamageEnemy(GameObject enemy)
    {
        if (enemy == null) return;

        var enemyHealth = enemy.GetComponent<EnemyHealth>();
        Assert.IsNotNull(enemyHealth, "Enemy should have an EnemyHealth component.");

        var damageAmount = (int)player.Stats.CalculateWeaponStat(StatType.Attack, this);
        PersistentData.Instance.levelStats.totalDamageDealt += damageAmount;
        enemyHealth.Remove(damageAmount);

        if (effect != null)
            effect.Apply(enemy);
    }

    private void PerformAttack()
    {
        Invoke(nameof(PerformAttack), player.Stats.CalculateWeaponStat(StatType.Cooldown, this));
        Attack();
    }

    public float GetStatValue(StatType type)
    {
        if (_stats.TryGetValue(type, out Stat stat)) 
            return stat.GetValue();

        return 0f;
    }
}
