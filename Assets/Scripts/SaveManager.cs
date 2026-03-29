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

    public SaveData saveData = new SaveData();
    private string saveFilePath;

    private void Awake()
    {
        // just in case
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
}

[System.Serializable]
public class SaveData
{
    //Special Resources
    public List<ResourceEntry> specialResources = new List<ResourceEntry>();
    [System.NonSerialized]
    public Dictionary<string, int> resourceMap = new Dictionary<string, int>();

    public void AddResource(ResourceData data, int amount)
    {
        string id = data.resourceName;

        if(resourceMap.ContainsKey(id))
            resourceMap[id] += amount;
        else
            resourceMap[id] = amount;
    }

    public void SyncMapFromList()
    {
        resourceMap.Clear();
        foreach (var entry in specialResources) resourceMap[entry.key] = entry.value;
    }

    public void SyncListFromMap()
    {
        specialResources.Clear();
        foreach (var kv in resourceMap) specialResources.Add(new ResourceEntry(kv.Key, kv.Value));
    }
}