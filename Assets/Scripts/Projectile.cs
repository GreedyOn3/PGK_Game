using UnityEngine;
using UnityEngine.Assertions;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5.0f;
    [SerializeField] private int damage = 5;
    private bool _active = true;

    private void Awake()
    {
        Invoke(nameof(DestroyOnLifetimeOver), lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_active)
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            var enemyHealth = other.GetComponent<EnemyHealth>();
            Assert.IsNotNull(enemyHealth, "Enemy should have an EnemyHealth component.");
            enemyHealth.Remove(damage);
            _active = false;
            Destroy(gameObject);
        }
    }

    private void DestroyOnLifetimeOver()
    {
        Destroy(gameObject);
    }
}
