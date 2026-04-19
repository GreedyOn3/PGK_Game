public class Util
{
    public static string FormatLevelTime(int minutes, int seconds)
    {
        var text = "";
        text += minutes < 10 ? $"0{minutes}" : $"{minutes}";
        text += ":";
        text += seconds < 10 ? $"0{seconds}" : $"{seconds}";
        return text;
    }
}
