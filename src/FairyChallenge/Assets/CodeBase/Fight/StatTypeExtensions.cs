using Savidiy.Utils;

namespace Fight
{
    public static class StatTypeExtensions
    {
        private static readonly EnumStringCache<StatType> Cache = new();

        public static string ToStringCashed(this StatType value)
        {
            return Cache.ToString(value);
        }
    }
}