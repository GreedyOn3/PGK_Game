using UnityEngine;

public abstract class Weapon : Armament
{
    public WeaponInfo weaponInfo;
    [SerializeField] protected float cooldownSeconds = 5.0f;

    public abstract void Attack();

    private void Start()
    {
        Invoke(nameof(PerformAttack), cooldownSeconds);
    }

    private void PerformAttack()
    {
        Invoke(nameof(PerformAttack), cooldownSeconds);
        Attack();
    }
}
