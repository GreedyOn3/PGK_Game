using UnityEngine;
using UnityEngine.Assertions;

// Abstract class representing a projectile or weapon that can damage an enemy.
public class Armament : MonoBehaviour
{
    [SerializeField] protected float damageScaling = 1.0f;
    [SerializeField] protected EffectParams effect;

    protected PlayerReferences player;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerReferences>();
    }

    protected void DamageEnemy(GameObject enemy)
    {
        if (enemy == null) return;

        var enemyHealth = enemy.GetComponent<EnemyHealth>();
        Assert.IsNotNull(enemyHealth, "Enemy should have an EnemyHealth component.");
        enemyHealth.Remove((int)(player.Stats.Attack * damageScaling));
        if (effect != null)
            effect.Apply(enemy);
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
