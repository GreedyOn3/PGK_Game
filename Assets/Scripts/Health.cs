using UnityEngine;

public class Health : MonoBehaviour
{
    public float value = 100.0f;
    public float maxValue = 100.0f;

    public void Add(float amount)
    {
        value = Mathf.Clamp(value + amount, 0.0f, maxValue);
    }
}
