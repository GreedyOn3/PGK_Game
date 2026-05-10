using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Weapon : Armament
{
    public WeaponInfo weaponInfo;

    protected PlayerReferences player;

    [SerializeField] private EffectParams effect;

    private Dictionary<StatType, Stat> _stats = new();

    public abstract void Attack();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerReferences>();
    }

    private void Start()
    {
        foreach (StatInfo statInfo in weaponInfo.BaseStats)
            _stats.Add(statInfo.Type, new Stat(statInfo.Value));

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

    public void AddModifier(StatType type, float value, bool isPercentage)
    {
        if (!_stats.TryGetValue(type, out Stat stat)) return;

        stat.AddModifier(new StatModifier(value, isPercentage));
    }

    protected GameObject FindNearestEnemy(float range)
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject nearestEnemy = null;
        var shortestDistance = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            var distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < shortestDistance && distance <= range)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
