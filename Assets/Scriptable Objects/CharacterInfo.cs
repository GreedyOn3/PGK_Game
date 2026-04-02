using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "Scriptable Objects/CharacterInfo")]
public class CharacterInfo : ScriptableObject
{
    public Sprite image;
    public string characterName;
    [TextArea(3, 5)]
    public string description;
    public CharacterId id;
}

public enum CharacterId
{
    Miner,
    Lumberjack,
}
