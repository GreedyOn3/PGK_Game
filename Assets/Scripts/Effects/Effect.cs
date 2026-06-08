using UnityEngine;

public class Effect : MonoBehaviour
{
    public float durationLeftSeconds = 5.0f;
    public GameObject associatedObject = null;

    protected virtual void FixedUpdate()
    {
        durationLeftSeconds -= Time.fixedDeltaTime;

        if (durationLeftSeconds <= 0.0f)
        {
            Destroy(this);

            if (associatedObject)
                Destroy(associatedObject);
        }
    }
}
