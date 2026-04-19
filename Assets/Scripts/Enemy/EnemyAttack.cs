using UnityEngine;
using UnityEngine.Assertions;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldownSeconds = 0.3f;
    [SerializeField] private int damage = 5;
    [SerializeField] private Vector3 hitBoxSize = new(1.5f, 1.0f, 3.0f);
    [SerializeField] private LayerMask playerLayer;

    private float _attackTimer;

    private void FixedUpdate()
    {
        _attackTimer += Time.fixedDeltaTime;

        if (_attackTimer < attackCooldownSeconds)
        {
            return;
        }

        var boxCenter = transform.position + transform.forward * (hitBoxSize.z / 2.0f);
        var hitColliders = Physics.OverlapBox(boxCenter, hitBoxSize / 2.0f, transform.rotation, playerLayer);

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                AttackPlayer(collider.gameObject);
                _attackTimer = 0.0f;
            }
        }
    }

    private void AttackPlayer(GameObject player)
    {
        var playerHealth = player.GetComponent<PlayerHealth>();
        var playerStats = player.GetComponent<PlayerStats>();
        Assert.IsNotNull(playerHealth, "Player should have a PlayerHealth component.");
        Assert.IsNotNull(playerStats, "Player should have a PlayerStats component.");
        var damageAmount = (int)(damage / (1.0f + playerStats.GetModifierValue(StatType.Defense) / 100.0f));
        PersistentData.Instance.levelStats.totalDamageTaken += damageAmount;
        playerHealth.Remove(damageAmount);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var boxCenter = transform.position + transform.forward * (hitBoxSize.z / 2.0f);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, hitBoxSize);
    }
}
