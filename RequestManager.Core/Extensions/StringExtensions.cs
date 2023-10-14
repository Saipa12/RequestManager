namespace BlackSun.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharacterToLower(this string @string) => char.ToLowerInvariant(@string[0]) + @string[1..];
    }
}