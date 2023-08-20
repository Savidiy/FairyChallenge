namespace Fairy
{
    public sealed class StatChangeData
    {
        public readonly Hero Target;
        public readonly StatType StatType;
        public readonly int Delta;

        public StatChangeData(Hero target, StatType statType, int delta)
        {
            Target = target;
            StatType = statType;
            Delta = delta;
        }

        public override string ToString()
        {
            return $"{StatType.ToStringCashed()} {Delta}";
        }
    }
}