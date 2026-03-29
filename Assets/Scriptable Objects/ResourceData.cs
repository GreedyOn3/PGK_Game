using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Scriptable Objects/Resource Data")]
public class ResourceData : ScriptableObject
{
    public string resourceName;
    public bool isSpecial;
    public Sprite icon;
}

[System.Serializable]
public class ResourceEntry
{
    public string key;
    public int value;
    public ResourceEntry(string k, int v) { key = k; value = v; }
}
