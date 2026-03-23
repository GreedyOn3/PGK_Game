using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float value = 100.0f;
    [SerializeField] private float maxValue = 100.0f;

    public float Value => value;
    public float MaxValue => maxValue;

    public void Add(float amount)
    {
        value = Mathf.Clamp(value + amount, 0.0f, maxValue);
    }
}
