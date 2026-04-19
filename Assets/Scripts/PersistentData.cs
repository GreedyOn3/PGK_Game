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

public struct LevelStats
{
    public int survivedTimeMinutes;
    public int survivedTimeSeconds;
    public int playerLevelReached;
    public int totalDamageDealt;
    public int totalDamageTaken;
    public int weaponsCollected;
    public int passivesCollected;

    public string GetStatsText()
    {
        var text = "";
        text += $"Time survived: {Util.FormatLevelTime(survivedTimeMinutes, survivedTimeSeconds)}\n";
        text += $"Level reached: {playerLevelReached}\n";
        text += $"Total damage dealt: {totalDamageDealt}\n";
        text += $"Total damage taken: {totalDamageTaken}\n";
        text += $"Weapons collected: {weaponsCollected}\n";
        text += $"Passives collected: {passivesCollected}\n";
        return text;
    }
}
