using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPrefabs", menuName = "Scriptable Objects/Player Prefabs")]
public class PlayerPrefabs : ScriptableObject
{
    public GameObject playerCamera;

    public GameObject minerPrefab;
    public GameObject lumberjackPrefab;

    public GameObject GetById(CharacterId id)
    {
        switch (id)
        {
            case CharacterId.Miner:
                return minerPrefab;
            case CharacterId.Lumberjack:
                return lumberjackPrefab;
            default:
                throw new ArgumentOutOfRangeException(nameof(id), id, null);
        }
    }
}
