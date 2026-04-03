using UI;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int value = 10;
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private Vector3 damagePopupSpawnOffset = new(0.0f, 0.5f, 0.0f);

    public int Value => value;

    public void Remove(int amount)
    {
        value -= amount;
        SpawnDamagePopup(amount);

        if (value <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnDamagePopup(int amount)
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
