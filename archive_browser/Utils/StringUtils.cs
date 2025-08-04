namespace archive_browser.Utils;

public static class StringUtils
{
    public static string FirstCharToUpper(string? s)
    {
        if (string.IsNullOrEmpty(s))
            return "";

        s = s.ToLower();
        return char.ToUpper(s[0]) + s[1..];
    }
}