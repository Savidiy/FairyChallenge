using System;
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
            attackResult.SetAttackId(attackData.AttackId);
            var damage = 0;

            foreach (EffectStaticData attackDataEffect in attackData.Effects)
            {
                int power = attackDataEffect.Power;
                EffectType effectType = attackDataEffect.EffectType;
                StatChangeData statChangeData = effectType switch
                {
                    EffectType.Damage => CalcDamage(attacker, defender, power),
                    EffectType.Heal => CalcHeal(attacker, power),
                    EffectType.HealByDamage => CalcHeal(attacker, power, damage),
                    EffectType.BuffSelfAttack => CalcBuff(StatType.Attack, attacker, power),
                    EffectType.BuffSelfDefence => CalcBuff(StatType.Defence, attacker, power),
                    EffectType.DebuffEnemyAttack => CalcBuff(StatType.Attack, defender, -power),
                    EffectType.DebuffEnemyDefence => CalcBuff(StatType.Defence, defender, -power),
                    _ => throw new Exception($"Unknown effect type '{effectType}'")
                };
                attackResult.AddChange(statChangeData);
                if (effectType == EffectType.Damage)
                {
                    var effectiveDamage = Mathf.Min(defender.Stats.Get(StatType.HealthPoints), -statChangeData.Delta);
                    damage += effectiveDamage;
                }
            }

            return attackResult;
        }

        private static StatChangeData CalcHeal(Hero attacker, int power)
        {
            int baseValue = attacker.Stats.Get(StatType.MaxHealthPoints);
            return CalcHeal(attacker, power, baseValue);
        }

        private static StatChangeData CalcHeal(Hero attacker, int power, int baseValue)
        {
            int value = Ceil(baseValue * power / 100f);
            return new StatChangeData(attacker, StatType.HealthPoints, value);
        }

        private static StatChangeData CalcBuff(StatType statType, Hero target, int percent)
        {
            int value = Ceil(target.Stats.Get(statType) * percent / 100f);
            return new StatChangeData(target, statType, value);
        }

        private static int Ceil(float value)
        {
            return Mathf.CeilToInt(value);
        }

        private static bool IsIndexOutOfRange(Hero attacker, int heroAttackIndex)
        {
            int availableAttacksCount = attacker.AvailableAttacks.Count;
            if (heroAttackIndex < availableAttacksCount)
                return false;

            Debug.LogError($"Attack index '{heroAttackIndex}' out of range '{availableAttacksCount}'");
            return true;
        }

        private StatChangeData CalcDamage(Hero attacker, Hero defender, int power)
        {
            int attackerAttack = attacker.Stats.Get(StatType.Attack);
            int defenderDefence = defender.Stats.Get(StatType.Defence);
            float defenceMultiplier = _fightSettings.DefenceMultiplier;
            float defenceBonus = Mathf.Max(0f, defenceMultiplier * (defenderDefence - attackerAttack));
            int damage = Mathf.CeilToInt(power * attackerAttack / (defenderDefence + defenceBonus));

            var statChangeData = new StatChangeData(defender, StatType.HealthPoints, -damage);
            return statChangeData;
        }
    }
}