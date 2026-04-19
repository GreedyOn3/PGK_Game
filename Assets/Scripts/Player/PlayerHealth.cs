public class PlayerHealth : Health
{
    protected override void OnZeroHealth()
    {
        LevelManager.Instance.GameOver();
    }
}
