using UnityEngine;

public class Lifetime : MonoBehaviour
{
    public float lifetime = 5.0f;

    private void Awake()
    {
        Invoke(nameof(DestroyOnLifetimeOver), lifetime);
    }

    private void DestroyOnLifetimeOver()
    {
        Destroy(gameObject);
    }
}
