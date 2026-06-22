using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SaveManager>();

                if (_instance == null)
                {
                    GameObject singletonObj = new GameObject("SaveManager (AutoSpawned)");
                    _instance = singletonObj.AddComponent<SaveManager>();
                }
            }
            return _instance;
        }
    }

    public SaveData saveData = new();
    private string saveFilePath;

    private void Awake()
    {
        if (_instance && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "save_file.json");
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        saveData.SyncListFromMap();
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
            saveData.SyncMapFromList();
        }
    }

    public void DeleteSavegame()
    {
        if (File.Exists(saveFilePath))
            File.WriteAllText(saveFilePath, "");
        saveData = new();
    }
}

[System.Serializable]
public class SaveData
{
    // Special Resources
    public List<ResourceEntry> specialResources = new();
    [System.NonSerialized]
    public Dictionary<string, int> resourceMap = new();

    // Permanent Upgrades
    public List<PermanentUpgradeEntry> permanentUpgrades = new();
    [System.NonSerialized]
    public Dictionary<string, PermanentUpgradeEntry.Value> permanentUpgradeMap = new();

    // Achievements
    public List<AchievementEntry> achievements = new();
    [System.NonSerialized]
    public Dictionary<string, bool> achievementMap = new();

    public void AddResource(ResourceData data, int amount)
    {
        string id = data.resourceName;

        if (resourceMap.ContainsKey(id))
            resourceMap[id] += amount;
        else
            resourceMap[id] = amount;
    }

    public int GetTotalSpecialResources()
    {
        int total = 0;
        foreach (KeyValuePair<string, int> kvp in resourceMap)
            total += kvp.Value;

        return total;
    }

    public void DecreaseSpecialResources(int amount)
    {
        int remaining = amount;
        List<string> keys = new List<string>(resourceMap.Keys);

        foreach (string key in keys)
        {
            if (remaining <= 0) break;

            int available = resourceMap[key];
            if (available > 0)
            {
                int deducted = Mathf.Min(available, remaining);
                resourceMap[key] -= deducted;
                remaining -= deducted;
            }
        }
    }

    public void SavePermanentUpgrade(PermanentUpgradeInfo upgrade)
    {
        permanentUpgradeMap[upgrade.UpgradeName] = new PermanentUpgradeEntry.Value
        {
            bought = upgrade.bought,
            enabled = upgrade.enabled,
        };
    }

    public void LoadPermanentUpgrade(PermanentUpgradeInfo upgrade)
    {
        if (permanentUpgradeMap.ContainsKey(upgrade.UpgradeName))
        {
            var value = permanentUpgradeMap[upgrade.UpgradeName];
            upgrade.bought = value.bought;
            upgrade.enabled = value.enabled;
        }
        else
        {
            upgrade.ResetUpgrade();
        }
    }

    public void SaveAchievement(AchievementInfo achievement)
    {
        achievementMap[achievement.AchievementName] = achievement.unlocked;
    }

    public void LoadAchievement(AchievementInfo achievement)
    {
        if (achievementMap.ContainsKey(achievement.AchievementName))
        {
            var value = achievementMap[achievement.AchievementName];
            achievement.unlocked = value;
        }
        else
        {
            achievement.ResetAchievement();
        }
    }

    public void SyncMapFromList()
    {
        resourceMap.Clear();
        foreach (var entry in specialResources) resourceMap[entry.key] = entry.value;
        permanentUpgradeMap.Clear();
        foreach (var entry in permanentUpgrades) permanentUpgradeMap[entry.key] = entry.value;
        achievementMap.Clear();
        foreach (var entry in achievements) achievementMap[entry.key] = entry.value;
    }

    public void SyncListFromMap()
    {
        specialResources.Clear();
        foreach (var kv in resourceMap) specialResources.Add(new ResourceEntry(kv.Key, kv.Value));
        permanentUpgrades.Clear();
        foreach (var kv in permanentUpgradeMap) permanentUpgrades.Add(new PermanentUpgradeEntry(kv.Key, kv.Value));
        achievements.Clear();
        foreach (var kv in achievementMap) achievements.Add(new AchievementEntry(kv.Key, kv.Value));
    }
}