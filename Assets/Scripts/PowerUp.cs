using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject visual;
    [Header("Stat Info")]
    public StatType statType;
    public float amount = 100f;
    public float duration = 5f;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerStats>(out PlayerStats stats))
        {
            //stats.AddModifier(statType, new StatModifier(amount, true, Time.time + duration));
            stats.ApplyBuff(statType, amount, duration);
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if (!_camera) return;
        visual.transform.LookAt(visual.transform.position + _camera.transform.forward);
    }
}
