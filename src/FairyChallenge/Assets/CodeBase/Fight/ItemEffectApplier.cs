using System;

namespace Fight
{
    public sealed class ItemEffectApplier
    {
        private readonly HeroStats _heroStats;
        private readonly HeroActions _heroActions;
        private readonly ActionFactory _actionFactory;

        public ItemEffectApplier(HeroStats heroStats, HeroActions heroActions, ActionFactory actionFactory)
        {
            _heroStats = heroStats;
            _heroActions = heroActions;
            _actionFactory = actionFactory;
        }

        public void ApplyItemEffects(Item item)
        {
            foreach (ItemEffect itemEffect in item.ItemStaticData.Effects)
            {
                switch (itemEffect.ItemEffectType)
                {
                    case ItemEffectType.AddAction:
                        string actionId = itemEffect.ActionId;
                        ActionData actionData = _actionFactory.Create(actionId);
                        _heroActions.AddAction(actionData);
                        break;
                    case ItemEffectType.ChangeStat:
                        _heroStats.ApplyChange(new StatChangeData(null, ConvertType(itemEffect.StatType), itemEffect.Value));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void RemoveItemEffects(Item item)
        {
            foreach (ItemEffect itemEffect in item.ItemStaticData.Effects)
            {
                switch (itemEffect.ItemEffectType)
                {
                    case ItemEffectType.AddAction:
                        _heroActions.RemoveAction(itemEffect.ActionId);
                        break;
                    case ItemEffectType.ChangeStat:
                        _heroStats.ApplyChange(new StatChangeData(null, ConvertType(itemEffect.StatType), -itemEffect.Value));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private StatType ConvertType(ItemStatType itemStatType)
        {
            return itemStatType switch
            {
                ItemStatType.Attack => StatType.Attack,
                ItemStatType.Defence => StatType.Defence,
                ItemStatType.HealthPoints => StatType.MaxHealthPoints,
                _ => throw new Exception($"Unknown stat type '{itemStatType}'")
            };
        }
    }
}