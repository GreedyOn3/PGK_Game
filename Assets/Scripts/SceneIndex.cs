using System;

public class SceneIndex
{
    public static readonly string MainMenu = "MainMenu";
    public static readonly string GameplayScene = "GameplayScene";
    public static readonly string LevelForest = "LevelForest";

    public static string GetByLevelId(LevelId id)
    {
        switch (id)
        {
            case LevelId.Forest:
                return LevelForest;
            default:
                throw new ArgumentOutOfRangeException(nameof(id), id, null);
        }
    }
}