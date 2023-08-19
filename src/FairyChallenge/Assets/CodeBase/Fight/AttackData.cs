namespace Fight
{
    public sealed class AttackData
    {
        private readonly AttackStaticData _attackStaticData;
        public readonly int FromLevel;
        public int Power => _attackStaticData.Power;

        public AttackData(AttackStaticData attackStaticData, int fromLevel)
        {
            _attackStaticData = attackStaticData;
            FromLevel = fromLevel;
        }
    }
}