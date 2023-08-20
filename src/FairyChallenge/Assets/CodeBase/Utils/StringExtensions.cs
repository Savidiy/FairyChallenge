namespace Savidiy.Utils
{
    public static class StringExtensions
    {
        public static string Color(this string text, string color)
        {
            return $"<color={color}>{text}</color>";
        }
    }
}