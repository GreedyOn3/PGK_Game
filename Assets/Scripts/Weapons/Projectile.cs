using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Lifetime))]
public class Projectile : Armament
{
    private bool _active = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!_active)
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            DamageEnemy(other.gameObject);
            _active = false;
            Destroy(gameObject);
        }
    }
}
