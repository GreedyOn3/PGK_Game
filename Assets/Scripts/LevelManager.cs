using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Infos")]
    [SerializeField] private LevelInfo forestLevelInfo;

    [Header("Player Prefabs")]
    [SerializeField] private GameObject minerPrefab;
    [SerializeField] private GameObject lumberjackPrefab;

    [Header("Player Camera")]
    [SerializeField] private GameObject playerCameraPrefab;

    private LevelInfo _levelInfo;

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

        switch (persistentData.selectedLevel)
        {
            case LevelId.Forest:
                _levelInfo = forestLevelInfo;
                SceneManager.LoadScene(SceneIndex.LevelForest, LoadSceneMode.Additive);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        GameObject player = null;

        switch (persistentData.selectedCharacter)
        {
            case CharacterId.Miner:
                player = Instantiate(minerPrefab);
                break;
            case CharacterId.Lumberjack:
                player = Instantiate(lumberjackPrefab);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var playerCamera = Instantiate(playerCameraPrefab);
        var playerCameraComponent = player.GetComponent<PlayerCamera>();
        playerCameraComponent.playerCamera = playerCamera.transform;

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
        if (LevelTimeSeconds > _levelInfo.timeLimitMinutes * 60.0f)
        {
            // TODO: Game over screen.
            SceneManager.LoadScene(SceneIndex.MainMenu);
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

    public string GetLevelName()
    {
        return _levelInfo.levelName;
    }

    public float GetLevelTimeLimitMinutes()
    {
        return _levelInfo.timeLimitMinutes;
    }
}
