using UnityEngine;

[CreateAssetMenu(fileName = "AchievementInfo", menuName = "Scriptable Objects/AchievementInfo")]
public class AchievementInfo : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public string AchievementName { get; private set; }
    [TextArea(3, 5)]
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public AchievementId Id { get; private set; }
    public bool unlocked;

    public void Unlock()
    {
        unlocked = true;
    }

    public void ResetAchievement()
    {
        unlocked = false;
    }
}

[System.Serializable]
public class AchievementEntry
{
    public string key;
    public bool value;

    public AchievementEntry(string k, bool v) { key = k; value = v; }
}

public enum AchievementId
{
    ReachLevel5,
    ReachLevel10,
}
