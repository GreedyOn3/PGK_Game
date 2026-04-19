using UnityEngine;

public class ShootingWeapon : Weapon
{
    [SerializeField] private float range = 15.0f;
    [SerializeField] private float projectileSpeed = 20.0f;
    [SerializeField] private GameObject projectilePrefab;

    public override void Attack()
    {
        var enemy = FindNearestEnemy(range);

        if (enemy != null)
        {
            Shoot(enemy);
        }
    }

    private void Shoot(GameObject enemy)
    {
        var firePosition = transform.position;
        var direction = (enemy.transform.position - firePosition).normalized;
        var projectile = Instantiate(projectilePrefab, firePosition, Quaternion.identity);
        projectile.GetComponent<Projectile>().Init(this);
        projectile.transform.forward = direction;

        var rigidBody = projectile.GetComponent<Rigidbody>();
        rigidBody.linearVelocity = direction * projectileSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
