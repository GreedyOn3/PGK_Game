using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfos", menuName = "Scriptable Objects/Level Infos")]
public class LevelInfos : ScriptableObject
{
    public LevelInfo forestLevelInfo;

    public LevelInfo GetById(LevelId id)
    {
        switch (id)
        {
            case LevelId.Forest:
                return forestLevelInfo;
            default:
                throw new ArgumentOutOfRangeException(nameof(id), id, null);
        }
    }
}
