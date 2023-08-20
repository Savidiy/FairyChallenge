using System.Collections.Generic;

namespace Fairy
{
    public class HeroFactory
    {
        private readonly HeroLibrary _heroLibrary;
        private readonly ActionFactory _actionFactory;
        private readonly ItemFactory _itemFactory;

        public HeroFactory(HeroLibrary heroLibrary, ActionFactory actionFactory, ItemFactory itemFactory)
        {
            _heroLibrary = heroLibrary;
            _actionFactory = actionFactory;
            _itemFactory = itemFactory;
        }

        public Hero Create(string heroId)
        {
            var heroData = _heroLibrary.GetStaticData(heroId);
            IReadOnlyList<ActionData> actions = _actionFactory.Create(heroData.Actions);
            var heroActions = new HeroActions(actions);
            var heroStats = new HeroStats(heroData);
            var itemEffectApplier = new ItemEffectApplier(heroStats, heroActions, _actionFactory);
            var inventory = new Inventory(itemEffectApplier);
            var hero = new Hero(heroData, heroActions, heroStats, inventory);
            return hero;
        }

        public Hero Create(string heroId, List<AdditionalActionData> additionalActions,
            List<SelectedConsumablesData> consumablesData)
        {
            Hero hero = Create(heroId);
            foreach (AdditionalActionData additionalActionData in additionalActions)
                hero.HeroActions.AddAction(_actionFactory.Create(additionalActionData.ActionId));

            foreach (SelectedConsumablesData selectedConsumablesData in consumablesData)
            {
                string itemId = selectedConsumablesData.ItemId;
                Item item = _itemFactory.Create(itemId);
                hero.Inventory.AddItem(item);
            }
            return hero;
        }
    }
}