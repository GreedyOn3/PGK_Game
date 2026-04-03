using UnityEngine;

public class XpPickup : MonoBehaviour
{
    public int amount = 5;

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
