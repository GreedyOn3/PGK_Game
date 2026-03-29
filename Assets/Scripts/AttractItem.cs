using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AttractItem : MonoBehaviour
{
    public float attractionForce = 3.0f;

    private GameObject _player;
    private PlayerStats _playerStats;
    private Rigidbody _rigidbody;

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
            var linearAttractionModifier = (1.0f - distanceToPlayer / _playerStats.PickupRange);
            var quadraticAttractionModifier = linearAttractionModifier * linearAttractionModifier;
            _rigidbody.AddForce(playerDirection * (attractionForce * quadraticAttractionModifier), ForceMode.Impulse);
        }
    }
}
