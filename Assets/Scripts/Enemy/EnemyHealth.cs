public class EnemyHealth : Health
{
    protected override void OnZeroHealth()
    {
        Destroy(gameObject);
    }
}
