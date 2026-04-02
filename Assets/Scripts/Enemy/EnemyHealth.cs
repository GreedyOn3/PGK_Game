using UI;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float value = 10.0f;
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private Vector3 damagePopupSpawnOffset = new(0.0f, 0.5f, 0.0f);

    public float Value => value;

    public void Remove(float amount)
    {
        value -= amount;
        SpawnDamagePopup(amount);

        if (value <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnDamagePopup(float amount)
    {
        var popupPosition = transform.position + damagePopupSpawnOffset;
        // Add a random offset to the spawn position.
        popupPosition += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        var worldSpaceCanvas = GameObject.FindGameObjectWithTag("World Space Canvas");
        var damagePopup = Instantiate(damagePopupPrefab, worldSpaceCanvas.transform);
        damagePopup.transform.position = popupPosition;
        damagePopup.GetComponent<DamagePopup>().SetDamage(amount);
    }
}
