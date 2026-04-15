using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public static PersistentData Instance { get; private set; }

    public CharacterInfo selectedCharacter;
    public LevelInfo selectedLevel;

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

        DontDestroyOnLoad(gameObject);
    }
}
