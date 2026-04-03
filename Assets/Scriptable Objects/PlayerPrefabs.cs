using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPrefabs", menuName = "Scriptable Objects/Player Prefabs")]
public class PlayerPrefabs : ScriptableObject
{
    [field: SerializeField] public GameObject PlayerCamera { get; private set; }

    [field: SerializeField] public GameObject MinerPrefab { get; private set; }
    [field: SerializeField] public GameObject LumberjackPrefab { get; private set; }

    public GameObject GetById(CharacterId id)
    {
        switch (id)
        {
            case CharacterId.Miner:
                return MinerPrefab;
            case CharacterId.Lumberjack:
                return LumberjackPrefab;
            default:
                throw new ArgumentOutOfRangeException(nameof(id), id, null);
        }
    }
}
