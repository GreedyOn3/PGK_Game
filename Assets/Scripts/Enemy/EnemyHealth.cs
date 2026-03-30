using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float value = 10.0f;

    public float Value => value;

    public void Remove(float amount)
    {
        value -= amount;

        if (value <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
