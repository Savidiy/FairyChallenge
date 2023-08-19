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
        
        public List<AttackData> Create(List<AvailableAttackStaticData> attacks)
        {
            var result = new List<AttackData>();
            foreach (var attack in attacks)
            {
                AttackData attackInstance = Create(attack.AttackId, attack.AvailableFromLevel);
                result.Add(attackInstance);
            }

            return result;
        }

        public AttackData Create(string attackId, int availableFromLevel)
        {
            var attackData = _attackLibrary.GetStaticData(attackId);
            var attackInstance = new AttackData(attackData, availableFromLevel);
            return attackInstance;
        }
    }
}