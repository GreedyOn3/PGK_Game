using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public static PersistentData Instance { get; private set; }

    public CharacterInfo selectedCharacter;
    public LevelInfo selectedLevel;
    public PermanentUpgradeInfo[] permanentUpgrades;
    public LevelStats levelStats = new();

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

    private void Start()
    {
        var saveData = SaveManager.instance.saveData;

        foreach (var upgrade in permanentUpgrades)
        {
            saveData.LoadPermanentUpgrade(upgrade);
        }
    }
}
