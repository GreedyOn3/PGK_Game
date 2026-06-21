public class PlayerHealth : Health
{
    protected override void OnZeroHealth()
    {
        if(LevelManager.Instance) LevelManager.Instance.GameOver(false);
    }
}
