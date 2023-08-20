using System.Collections.Generic;

namespace Fight
{
    public sealed class AttackData
    {
        private readonly AttackStaticData _attackStaticData;
        public IReadOnlyList<EffectStaticData> Effects => _attackStaticData.Effects;
        public string AttackId => _attackStaticData.AttackId;

        public AttackData(AttackStaticData attackStaticData)
        {
            _attackStaticData = attackStaticData;
        }
    }
}