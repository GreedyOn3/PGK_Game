using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Vector2 spawnRadiusMinMax = new(30.0f, 50.0f);
    [SerializeField] private List<RegularSpawn> regularSpawns = new();
    [SerializeField] private List<Wave> waves = new();

    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        var levelTimeMinutes = LevelManager.Instance.LevelTimeMinutes;

        for (var i = regularSpawns.Count - 1; i >= 0; i--)
        {
            var regularSpawn = regularSpawns[i];
            regularSpawn.SpawnTimer += Time.fixedDeltaTime;

            var spawnPeriodMin = regularSpawn.spawnPeriodMinutes.x;
            var spawnPeriodMax = regularSpawn.spawnPeriodMinutes.y;

            if (levelTimeMinutes >= spawnPeriodMin && levelTimeMinutes <= spawnPeriodMax)
            {
                if (regularSpawn.SpawnTimer >= regularSpawn.spawnIntervalSeconds)
                {
                    SpawnEnemy(regularSpawn.enemyPrefab);
                    regularSpawn.SpawnTimer = 0.0f;
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
        var position = GetRandomSpawnPosition();
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        var randomHorizontalDirection = Random.insideUnitCircle.normalized;
        var randomDirection = new Vector3(randomHorizontalDirection.x, 0.0f, randomHorizontalDirection.y);
        var randomDistance = Random.Range(spawnRadiusMinMax.x, spawnRadiusMinMax.y);
        var spawnPosition = _player.transform.position + randomDirection * randomDistance;
        spawnPosition.y = _player.transform.position.y;

        return spawnPosition;
    }
}

[System.Serializable]
public class RegularSpawn
{
    public GameObject enemyPrefab;
    public float spawnIntervalSeconds;
    public Vector2 spawnPeriodMinutes;
    [System.NonSerialized] public float SpawnTimer;
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
