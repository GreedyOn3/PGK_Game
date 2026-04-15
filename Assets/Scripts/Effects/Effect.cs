using UnityEngine;

public class Effect : MonoBehaviour
{
    public float durationLeftSeconds = 5.0f;

    protected virtual void FixedUpdate()
    {
        durationLeftSeconds -= Time.fixedDeltaTime;

        if (durationLeftSeconds <= 0.0f)
        {
            Destroy(this);
        }
    }
}
