using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponInfo weaponInfo;
    public PlayerReferences player;
    public float cooldownSeconds = 5.0f;

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
