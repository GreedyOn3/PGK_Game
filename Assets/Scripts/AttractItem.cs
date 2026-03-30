using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AttractItem : MonoBehaviour
{
    public float attractionForce = 10.0f;
    public float attractionForceSpeedup = 3.0f;

    private GameObject _player;
    private PlayerStats _playerStats;
    private Rigidbody _rigidbody;

    private float _attractionTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerStats = _player.GetComponent<PlayerStats>();
    }

    private void FixedUpdate()
    {
        var toPlayer = _player.transform.position - transform.position;
        var distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer < _playerStats.PickupRange)
        {
            var playerDirection = toPlayer.normalized;
            var attractionModifier = _attractionTime * _attractionTime * attractionForceSpeedup;
            _rigidbody.AddForce(playerDirection * (attractionForce * attractionModifier), ForceMode.Impulse);
            _attractionTime += Time.fixedDeltaTime;
        }
        else
        {
            _attractionTime = 0.0f;
        }
    }
}
