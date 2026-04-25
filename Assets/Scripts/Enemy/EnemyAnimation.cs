using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetSpeed(float amount)
    {
        if (animator == null) return;

        animator.SetFloat("Speed", amount);
    }

    public void SetAttack(bool attack)
    {
        if (animator == null) return;

        animator.SetBool("Attack", attack);
    }

    public bool IsAttackPlaying()
    {
        if (animator == null) return false;

        return animator.GetBool("Attack");
    }
}
