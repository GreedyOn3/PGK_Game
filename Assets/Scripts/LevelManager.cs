using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelInfo levelInfo;

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

        Time.timeScale = 1.0f;
    }

    private void Start()
    {
        var persistentData = PersistentData.Instance;
        Debug.Log($"Selected level: {persistentData.SelectedLevel}");
        Debug.Log($"Selected character: {persistentData.SelectedCharacter}");
    }

    private void FixedUpdate()
    {
        LevelTimeSeconds += Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (LevelTimeSeconds > levelInfo.timeLimitMinutes * 60.0f)
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
        return levelInfo.levelName;
    }

    public float GetLevelTimeLimitMinutes()
    {
        return levelInfo.timeLimitMinutes;
    }
}
