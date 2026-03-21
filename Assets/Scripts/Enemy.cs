using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float movementSpeed = 7.0f;

    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        var maxMovementDistance = movementSpeed * Time.fixedDeltaTime;
        var movementVector = Vector3.ClampMagnitude(_player.transform.position - transform.position, maxMovementDistance);
        transform.localPosition += movementVector;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
