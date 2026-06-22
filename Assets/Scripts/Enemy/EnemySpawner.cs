using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private MapObjectSpawner mapObjectSpawner;
    [SerializeField] private Vector2 spawnRadiusMinMax = new(30.0f, 50.0f);
    [SerializeField] private List<RegularSpawn> regularSpawns = new();
    [SerializeField] private List<Wave> waves = new();
    [Header("Shop")]
    [SerializeField] private GameObject shopPrefab;
    [SerializeField] private List<float> shopSpawnTimes = new();
    [SerializeField] private float shopDistance = 5f;

    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        float levelTimeMinutes = LevelManager.Instance.LevelTimeMinutes;

        for (int i = shopSpawnTimes.Count - 1; i >= 0; i--)
        {
            if(mapObjectSpawner && levelTimeMinutes >= shopSpawnTimes[i])
            {
                if(mapObjectSpawner.TryGetValidRandomPositionInRadius(shopPrefab, _player.transform, shopDistance, out Vector3 shopPos, out Vector3 shopNormal))
                    Instantiate(shopPrefab, shopPos, Quaternion.FromToRotation(Vector3.up, shopNormal));
                shopSpawnTimes.RemoveAt(i);
                break;
            }
        }

        for (var i = regularSpawns.Count - 1; i >= 0; i--)
        {
            var regularSpawn = regularSpawns[i];
            regularSpawn.spawnTimer += Time.deltaTime;

            var spawnPeriodMin = regularSpawn.spawnPeriodMinutes.x;
            var spawnPeriodMax = regularSpawn.spawnPeriodMinutes.y;

            if (levelTimeMinutes >= spawnPeriodMin && levelTimeMinutes <= spawnPeriodMax)
            {
                if (regularSpawn.spawnTimer >= regularSpawn.spawnIntervalSeconds)
                {
                    SpawnEnemy(regularSpawn.enemyPrefab);
                    regularSpawn.spawnTimer = 0.0f;
                }
            }
            else if (levelTimeMinutes > spawnPeriodMax)
            {
                regularSpawns.RemoveAt(i);
            }
        }

        for (var i = waves.Count - 1; i >= 0; i--)
        {
            var wave = waves[i];
            if (levelTimeMinutes >= wave.startTimeMinutes)
            {
                SpawnWave(wave);
                waves.RemoveAt(i);
            }
        }
    }

    private void SpawnWave(Wave wave)
    {
        foreach (var batch in wave.enemyBatches)
        {
            SpawnBatch(batch);
        }
    }

    private void SpawnBatch(EnemyBatch batch)
    {
        for (var i = 0; i < batch.enemyCount; i++)
        {
            SpawnEnemy(batch.enemyPrefab);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        var position = GetRandomSpawnPosition(enemyPrefab);
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition(GameObject enemyPrefab)
    {
        Vector3 spawnPosition = Vector3.zero;
        float randomDistance = Random.Range(spawnRadiusMinMax.x, spawnRadiusMinMax.y);

        if (!mapObjectSpawner)
        {
            var randomHorizontalDirection = Random.insideUnitCircle.normalized;
            var randomDirection = new Vector3(randomHorizontalDirection.x, 0.0f, randomHorizontalDirection.y);
            spawnPosition = _player.transform.position + randomDirection * randomDistance;
            spawnPosition.y = _player.transform.position.y;
        } 
        else
        {
            mapObjectSpawner.TryGetValidRandomPositionInRadius(enemyPrefab, _player.transform, randomDistance, out spawnPosition, out _);

            float yOffset = 0f;
            if (enemyPrefab.TryGetComponent<CapsuleCollider>(out CapsuleCollider col))
                yOffset = col.height / 2f * enemyPrefab.transform.localScale.y;
            spawnPosition.y += yOffset;
        }

        return spawnPosition;
    }
}

[System.Serializable]
public class RegularSpawn
{
    public GameObject enemyPrefab;
    public float spawnIntervalSeconds;
    public Vector2 spawnPeriodMinutes;
    [System.NonSerialized] public float spawnTimer;
}

[System.Serializable]
public class Wave
{
    public EnemyBatch[] enemyBatches;
    public float startTimeMinutes;
}

[System.Serializable]
public class EnemyBatch
{
    public GameObject enemyPrefab;
    public int enemyCount;
}
