using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject playerCameraPrefab;

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

        var persistentData = PersistentData.Instance;
        persistentData.levelStats = new LevelStats();

        _levelInfo = persistentData.selectedLevel;
        SceneManager.LoadScene(_levelInfo.LevelScene, LoadSceneMode.Additive);
        var playerPrefab = persistentData.selectedCharacter.Prefab;
        var player = Instantiate(playerPrefab);
        var playerCamera = Instantiate(playerCameraPrefab);
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

    private void FixedUpdate()
    {
        LevelTimeSeconds += Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (LevelTimeSeconds > _levelInfo.TimeLimitMinutes * 60.0f)
        {
            GameOver();
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

    public void GameOver()
    {
        var persistentData = PersistentData.Instance;
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
