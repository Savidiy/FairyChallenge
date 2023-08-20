using UnityEngine;

namespace Savidiy.Utils
{
    public static class StringExtensions
    {
        public static string Color(this string text, string color)
        {
            return $"<color={color}>{text}</color>";
        }
        public static string Color(this int text, string color)
        {
            return $"<color={color}>{text}</color>";
        }

        public static string Color(this string text, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
        }
    }
}