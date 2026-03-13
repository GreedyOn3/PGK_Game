using UnityEngine;

// If performance gets bad, we could consider object pooling.
[RequireComponent(typeof(BoxCollider))]
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnInterval = 5.0f;

    private BoxCollider _spawnArea;
    private float _spawnTimer;

    private void Start()
    {
        _spawnArea = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            _spawnTimer = 0.0f;
        }
    }

    public void SpawnEnemy()
    {
        var bounds = _spawnArea.bounds;
        var min = bounds.min;
        var max = bounds.max;
        var position = new Vector3(Random.Range(min.x, max.x), 0.0f, Random.Range(min.z, max.z));
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}
