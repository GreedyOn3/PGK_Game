using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelInfos levelInfos;
    [SerializeField] private PlayerPrefabs playerPrefabs;

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

        _levelInfo = levelInfos.GetById(persistentData.selectedLevel);
        SceneManager.LoadScene(SceneIndex.GetByLevelId(persistentData.selectedLevel), LoadSceneMode.Additive);
        var playerPrefab = playerPrefabs.GetById(persistentData.selectedCharacter);
        var player = Instantiate(playerPrefab);
        var playerCamera = Instantiate(playerPrefabs.playerCamera);
        player.GetComponent<PlayerCamera>().playerCamera = playerCamera.transform;

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
