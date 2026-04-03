using UnityEngine;

public class Lifetime : MonoBehaviour
{
    public float lifetime = 5.0f;

    private float _timer;

    private void FixedUpdate()
    {
        if (_timer > lifetime)
        {
            Destroy(gameObject);
        }

        _timer += Time.fixedDeltaTime;
    }
}
