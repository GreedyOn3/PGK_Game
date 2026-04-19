using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Lifetime))]
public class Projectile : MonoBehaviour
{
    private Weapon source;
    private bool _active = true;

    public void Init(Weapon source)
    {
        this.source = source;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_active)
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            if(source) source.DamageEnemy(other.gameObject);
            _active = false;
            Destroy(gameObject);
        }
    }
}
