using System;
using UnityEngine;

namespace Fairy
{
    public sealed class FightCalculator
    {
        private readonly FightSettings _fightSettings;

        public FightCalculator(FightSettings fightSettings)
        {
            _fightSettings = fightSettings;
        }
        
        public bool TryCalcResult(Hero attacker, int actionIndex, Hero defender, out ActionResult actionResult)
        {
            actionResult = new ActionResult();

            if (IsIndexOutOfRange(attacker, actionIndex))
                return false;
            
            if (actionIndex >= attacker.HeroActions.Count)
            {
                UseItem(attacker, actionIndex, actionResult);
                return true;
            }

            UseAction(attacker, defender, actionResult, actionIndex);

            return true;
        }

        private static void UseItem(Hero attacker, int heroAttackIndex, ActionResult actionResult)
        {
            int itemIndex = heroAttackIndex - attacker.HeroActions.Count;
            Item item = attacker.Inventory.TakeConsumable(itemIndex);

            string itemId = item.ItemStaticData.ItemId;
            actionResult.SetActionId(itemId);
            
            foreach (ItemEffect itemEffect in item.ItemStaticData.Effects)
            {
                if (itemEffect.ItemEffectType == ItemEffectType.AddAction)
                    Debug.LogError($"Consumable '{itemId}' can't add action");

                if (itemEffect.ItemEffectType == ItemEffectType.ChangeStat)
                {
                    int value = itemEffect.Value;
                    ItemStatType itemStatType = itemEffect.StatType;
                    var statType = itemStatType switch
                    {
                        ItemStatType.Attack => StatType.Attack,
                        ItemStatType.Defence => StatType.Defence,
                        ItemStatType.HealthPoints => StatType.HealthPoints,
                        _ => throw new Exception($"Unknown stat type '{itemStatType}'")
                    };

                    var statChangeData = new StatChangeData(attacker, statType, value);
                    actionResult.AddChange(statChangeData);
                }
            }
        }

        private void UseAction(Hero attacker, Hero defender, ActionResult actionResult, int actionIndex)
        {
            ActionData actionData = attacker.HeroActions.Actions[actionIndex];
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
            int actionsCount = attacker.HeroActions.Count + attacker.Inventory.Consumables.Count;
            return heroAttackIndex >= actionsCount;
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