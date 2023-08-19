using System.Collections.Generic;

namespace Fight
{
    public class AttackFactory
    {
        private readonly AttackLibrary _attackLibrary;

        public AttackFactory(AttackLibrary attackLibrary)
        {
            _attackLibrary = attackLibrary;
        }
        
        public IReadOnlyList<AttackData> Create(List<AvailableAttackStaticData> attacks)
        {
            var result = new List<AttackData>();
            foreach (var attack in attacks)
            {
                var attackData = _attackLibrary.GetStaticData(attack.AttackId);
                var attackInstance = new AttackData(attackData, attack.AvailableFromLevel);
                result.Add(attackInstance);
            }

            return result;
        }
    }
}