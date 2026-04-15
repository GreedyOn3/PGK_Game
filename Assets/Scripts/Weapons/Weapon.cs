using UnityEngine;

public abstract class Weapon : Armament
{
    public WeaponInfo weaponInfo;
    [SerializeField] protected float cooldownSeconds = 5.0f;

    public abstract void Attack();

    private void Start()
    {
        InvokeRepeating(nameof(PerformAttack), cooldownSeconds, cooldownSeconds);
    }

    private void PerformAttack()
    {
        Attack();
    }
}
