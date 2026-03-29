using UnityEngine;
using UnityEngine.Assertions;

namespace Weapons
{
    public class Wand : Weapon
    {
        [SerializeField] private float range = 15.0f;
        [SerializeField] private float projectileSpeed = 20.0f;
        [SerializeField] private GameObject wandProjectile;

        public override void Attack()
        {
            var enemy = FindNearestEnemy();

            if (enemy != null)
            {
                Shoot(enemy);
            }
        }

        private GameObject FindNearestEnemy()
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

        private void Shoot(GameObject enemy)
        {
            var firePosition = transform.position;
            var direction = (enemy.transform.position - firePosition).normalized;
            var projectile = Instantiate(wandProjectile, firePosition, Quaternion.identity);
            projectile.transform.forward = direction;

            var rigidBody = projectile.GetComponent<Rigidbody>();
            Assert.IsNotNull(rigidBody, "Wand projectile should have a rigidbody.");
            rigidBody.linearVelocity = direction * projectileSpeed;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
