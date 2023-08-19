using UnityEngine;

namespace Fight
{
    public sealed class FightCalculator
    {
        private readonly FightSettings _fightSettings;

        public FightCalculator(FightSettings fightSettings)
        {
            _fightSettings = fightSettings;
        }

        public AttackResult CalcAttack(Hero attacker, int heroAttackIndex, Hero defender)
        {
            var attackResult = new AttackResult();

            if (IsIndexOutOfRange(attacker, heroAttackIndex))
                return attackResult;

            AttackData attackData = attacker.AvailableAttacks[heroAttackIndex];
            int attackPower = attackData.Power;
            int attackerAttack = attacker.Stats.Get(StatType.Attack);
            int defenderDefence = defender.Stats.Get(StatType.Defence);
            float defenceMultiplier = _fightSettings.DefenceMultiplier;
            float defenceBonus = Mathf.Max(0f, defenceMultiplier * (defenderDefence - attackerAttack));
            int damage = Mathf.CeilToInt(attackPower * attackerAttack / (defenderDefence + defenceBonus));

            var statChangeData = new StatChangeData(StatType.CurrentHealthPoints, -damage);
            attackResult.AddChange(defender, statChangeData);
            return attackResult;
        }

        private static bool IsIndexOutOfRange(Hero attacker, int heroAttackIndex)
        {
            int availableAttacksCount = attacker.AvailableAttacks.Count;
            if (heroAttackIndex < availableAttacksCount)
                return false;

            Debug.LogError($"Attack index '{heroAttackIndex}' out of range '{availableAttacksCount}'");
            return true;
        }
    }
}