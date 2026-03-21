public abstract class Weapon
{
    public WeaponInfo Info { get; private set; }

    protected Weapon(WeaponInfo info)
    {
        Info = info;
    }

    public abstract void Attack();
}
