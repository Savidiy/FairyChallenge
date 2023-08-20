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

        public ActionResult CalcAction(Hero attacker, int heroAttackIndex, Hero defender)
        {
            var actionResult = new ActionResult();

            if (IsIndexOutOfRange(attacker, heroAttackIndex))
                return actionResult;

            ActionData actionData = attacker.Actions[heroAttackIndex];
            actionResult.SetActionId(actionData.ActionId);
            var damage = 0;

            foreach (EffectStaticData effectStaticData in actionData.Effects)
            {
                int power = effectStaticData.Power;
                EffectType effectType = effectStaticData.EffectType;
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
                actionResult.AddChange(statChangeData);
                if (effectType == EffectType.Damage)
                {
                    var effectiveDamage = Mathf.Min(defender.Stats.Get(StatType.HealthPoints), -statChangeData.Delta);
                    damage += effectiveDamage;
                }
            }

            return actionResult;
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
            int actionsCount = attacker.Actions.Count;
            if (heroAttackIndex < actionsCount)
                return false;

            Debug.LogError($"Action index '{heroAttackIndex}' out of range '{actionsCount}'");
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