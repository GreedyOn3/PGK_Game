using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfos", menuName = "Scriptable Objects/Level Infos")]
public class LevelInfos : ScriptableObject
{
    [field: SerializeField] public LevelInfo ForestLevelInfo { get; private set; }

    public LevelInfo GetById(LevelId id)
    {
        switch (id)
        {
            case LevelId.Forest:
                return ForestLevelInfo;
            default:
                throw new ArgumentOutOfRangeException(nameof(id), id, null);
        }
    }
}
