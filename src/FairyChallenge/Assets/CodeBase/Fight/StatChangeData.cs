namespace Fight
{
    public sealed class StatChangeData
    {
        public readonly StatType StatType;
        public readonly int Delta;

        public StatChangeData(StatType statType, int delta)
        {
            StatType = statType;
            Delta = delta;
        }

        public override string ToString()
        {
            return $"{StatType.ToStringCashed()} {Delta}";
        }
    }
}