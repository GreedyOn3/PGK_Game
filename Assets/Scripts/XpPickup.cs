using UnityEngine;

public class XpPickup : MonoBehaviour
{
    public float amount = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerXp = other.GetComponent<PlayerXp>();
            playerXp.Add(amount);
            Destroy(transform.parent.gameObject);
        }
    }
}
