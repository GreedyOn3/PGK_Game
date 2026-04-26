using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected int value = 10;
    [SerializeField] protected int maxValue = 10;
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private Vector3 damagePopupSpawnOffset = new(0.0f, 0.5f, 0.0f);

    public bool invincible = false;

    public int Value => value;
    public int MaxValue => maxValue;

    protected virtual void OnZeroHealth() {}

    public void Add(int amount)
    {
        value = Math.Clamp(value + amount, 0, maxValue);
    }

    public void Remove(int amount)
    {
        if (invincible)
            return;

        value = Math.Clamp(value - amount, 0, maxValue);
        SpawnDamagePopup(amount);

        if (value <= 0)
        {
            OnZeroHealth();
        }
    }

    private void SpawnDamagePopup(int damage)
    {
        var popupPosition = transform.position + damagePopupSpawnOffset;
        // Add a random offset to the spawn position.
        popupPosition += new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
        var worldSpaceCanvas = GameObject.FindGameObjectWithTag("World Space Canvas");
        var damagePopup = Instantiate(damagePopupPrefab, worldSpaceCanvas.transform);
        damagePopup.transform.position = popupPosition;
        var damagePopupComponent = damagePopup.GetComponent<UI.DamagePopup>();
        damagePopupComponent.SetDamage(damage);
    }
}
