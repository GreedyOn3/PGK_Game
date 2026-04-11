using UnityEngine;

namespace Weapons
{
    public class Whip : Weapon
    {
        [SerializeField] private Vector3 hitBoxSize = new(3.0f, 1.0f, 6.0f);
        [SerializeField] private LayerMask enemyLayer;

        public override void Attack()
        {
            var boxCenter = transform.position + transform.forward * (hitBoxSize.z / 2.0f);
            var hitColliders = Physics.OverlapBox(boxCenter, hitBoxSize / 2.0f, transform.rotation, enemyLayer);

            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    if(collider.gameObject) DamageEnemy(collider.gameObject);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var boxCenter = transform.position + transform.forward * (hitBoxSize.z / 2.0f);
            Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, hitBoxSize);
        }
    }
}
