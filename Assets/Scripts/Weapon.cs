using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public PlayerReferences player;

    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private float cooldownSeconds = 5.0f;

    public WeaponInfo WeaponInfo => weaponInfo;

    private void Start()
    {
        Invoke(nameof(DoAttack), cooldownSeconds);
    }

    private void DoAttack()
    {
        Invoke(nameof(DoAttack), cooldownSeconds);
        Attack();
    }

    public abstract void Attack();
}
