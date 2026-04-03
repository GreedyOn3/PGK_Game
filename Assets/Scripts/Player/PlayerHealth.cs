using UnityEngine;

public class PlayerHealth : Health
{
    protected override void OnZeroHealth()
    {
        // TODO: Game over.
        Debug.Log("PLAYER DEAD!");
    }
}
