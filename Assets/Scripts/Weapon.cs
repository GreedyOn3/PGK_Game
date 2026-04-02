using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private float cooldownSeconds = 5.0f;

    protected PlayerReferences Player;

    public WeaponInfo WeaponInfo => weaponInfo;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerReferences>();
        Invoke(nameof(DoAttack), cooldownSeconds);
    }

    private void DoAttack()
    {
        Invoke(nameof(DoAttack), cooldownSeconds);
        Attack();
    }

    public abstract void Attack();
}
