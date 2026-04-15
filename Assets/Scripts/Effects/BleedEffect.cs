public class BleedEffect : Effect
{
    public int healthReductionPerSecond = 1;

    private Health _health;

    private void Start()
    {
        _health = GetComponent<Health>();
        if (_health != null)
            InvokeRepeating(nameof(RemoveHealth), 1.0f, 1.0f);
    }

    private void RemoveHealth()
    {
        if (_health != null)
            _health.Remove(healthReductionPerSecond);
    }
}
