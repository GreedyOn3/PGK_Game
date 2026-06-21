using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float pushForce = 2.5f;
    public float attackDuration = 2f;
    public float attackRadius = 2f;
    public int attackDamage = 1;
    public List<GameObject> attackVisuals;

    public LayerMask playerLayer;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    public void Initialize(int damage, float duration, float radius, List<GameObject> visuals)
    {
        attackDamage = damage;
        attackDuration = duration;
        attackRadius = radius;
        attackVisuals = visuals;

        StartCoroutine(AnimateTelegraph());
    }

    private IEnumerator AnimateTelegraph()
    {
        float elapsedTime = 0f;
        transform.localScale = Vector3.zero;

        while (elapsedTime < attackDuration)
        {
            float progress = elapsedTime / attackDuration;
            transform.localScale = Vector3.Slerp(Vector3.zero, Vector3.one*attackRadius*2f, progress);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one * attackRadius * 2f;
        ExecuteBossAttack();
    }

    private void ExecuteBossAttack()
    {
        Debug.Log("Boss attack triggered!");
        if(attackVisuals.Count > 0)
        {
            GameObject randomPrefab = attackVisuals[Random.Range(0, attackVisuals.Count - 1)];
            GameObject attack = Instantiate(randomPrefab, transform.position, transform.rotation);
            attack.transform.localScale = randomPrefab.transform.localScale;

            Destroy(attack, 1f);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, playerLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            if(hitCollider.TryGetComponent<PlayerReferences>(out PlayerReferences player))
            {
                Vector3 toPlayer = (player.transform.position - transform.position).normalized;
                if (toPlayer.sqrMagnitude < 0.01f)
                    toPlayer = Random.onUnitSphere;

                player.Health.Remove(attackDamage);
                player.Movement.SetVelocity(toPlayer * pushForce);
            }
        }

        Destroy(gameObject);
    }
}
