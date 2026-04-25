public class Util
{
    public static string FormatLevelTime(int minutes, int seconds)
    {
        var text = $"{minutes:00}:{(seconds % 60):00}";
        return text;
    }
}
