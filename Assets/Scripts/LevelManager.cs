using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject playerCameraPrefab;
    [SerializeField] private MapObjectSpawner objectSpawner;

    private LevelInfo _levelInfo;
    private PlayerReferences _player;

    public float LevelTimeSeconds { get; private set; }
    public float LevelTimeMinutes => LevelTimeSeconds / 60.0f;
    public bool LevelPaused { get; private set; }

    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        PersistentData persistentData = PersistentData.Instance;
        persistentData.levelStats = new LevelStats();

        _levelInfo = persistentData.selectedLevel;
        SceneManager.LoadScene(_levelInfo.LevelScene, LoadSceneMode.Additive);
        GameObject playerPrefab = persistentData.selectedCharacter.Prefab;

        Vector3 spawnPos = Vector3.zero;
        if(objectSpawner)
            objectSpawner.TryGetValidRandomPosition(playerPrefab, out spawnPos, out Vector3 _);

        GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        GameObject playerCamera = Instantiate(playerCameraPrefab);
        player.GetComponent<PlayerCamera>().playerCamera = playerCamera.transform;
        _player = player.GetComponent<PlayerReferences>();

        Time.timeScale = 1.0f;
    }

    private void Start()
    {
        var persistentData = PersistentData.Instance;
        Debug.Log($"Selected character: {persistentData.selectedCharacter}");
        Debug.Log($"Selected level: {persistentData.selectedLevel}");
    }

    private void Update()
    {
        LevelTimeSeconds += Time.deltaTime;

        if (LevelTimeSeconds > _levelInfo.TimeLimitMinutes * 60.0f)
        {
            GameOver(true);
        }
    }

    public void PauseLevel()
    {
        Time.timeScale = 0.0f;
        LevelPaused = true;
    }

    public void UnpauseLevel()
    {
        Time.timeScale = 1.0f;
        LevelPaused = false;
    }

    public void GameOver(bool victory)
    {
        if (!PersistentData.Instance) return;

        var persistentData = PersistentData.Instance;
        persistentData.levelStats.victory = victory;
        persistentData.levelStats.survivedTimeMinutes = (int)LevelTimeMinutes;
        persistentData.levelStats.survivedTimeSeconds = (int)LevelTimeSeconds;
        persistentData.levelStats.playerLevelReached = _player.Xp.Level;
        persistentData.levelStats.weaponsCollected = _player.Inventory.GetWeaponCount();
        persistentData.levelStats.passivesCollected = _player.Inventory.GetPassivesCount();
        SceneManager.LoadScene("GameOver");
    }

    public string GetLevelName()
    {
        return _levelInfo.LevelName;
    }

    public float GetLevelTimeLimitMinutes()
    {
        return _levelInfo.TimeLimitMinutes;
    }
}
