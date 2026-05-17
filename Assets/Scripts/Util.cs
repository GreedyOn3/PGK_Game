using System.Collections.Generic;

public class Util
{
    public static string FormatLevelTime(int minutes, int seconds)
    {
        var text = $"{minutes:00}:{(seconds % 60):00}";
        return text;
    }

    public static T GetRandomWeighted<T>(List<T> items) where T : IWeighted
    {
        float totalWeight = 0f;
        foreach (T item in items)
            totalWeight += item.Weight;

        float randomPoint = UnityEngine.Random.Range(0, totalWeight);
        float current = 0f;
        foreach (T item in items)
        {
            current += item.Weight;
            if (randomPoint <= current)
                return item;
        }

        return items[^1];
    }
}

public interface IWeighted
{
    float Weight { get; }
}
