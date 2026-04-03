using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "Scriptable Objects/CharacterInfo")]
public class CharacterInfo : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public string CharacterName { get; private set; }

    [TextArea(3, 5)]
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public CharacterId Id { get; private set; }
}

public enum CharacterId
{
    Miner,
    Lumberjack,
}
