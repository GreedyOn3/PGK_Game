using UnityEngine;
using UnityEngine.Assertions;

namespace Weapons
{
    public class Wand : Weapon
    {
        public float range = 15.0f;
        public float projectileSpeed = 20.0f;
        public GameObject wandProjectile;

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
            if (rigidBody != null)
            {
                rigidBody.linearVelocity = direction * projectileSpeed;
            }
            else
            {
                Assert.IsTrue(false, "Wand projectile should have a rigidbody.");
            }
        }
    }
}
