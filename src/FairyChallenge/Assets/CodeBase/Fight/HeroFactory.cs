using System.Collections.Generic;

namespace Fight
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
            var hero = new Hero(heroData, actions);
            return hero;
        }

        public Hero Create(string heroId, List<AdditionalActionData> additionalActions,
            List<SelectedConsumablesData> consumablesData)
        {
            var heroData = _heroLibrary.GetStaticData(heroId);
            List<ActionData> actions = _actionFactory.Create(heroData.Actions);
            foreach (AdditionalActionData additionalActionData in additionalActions)
                actions.Add(_actionFactory.Create(additionalActionData.ActionId));

            var hero = new Hero(heroData, actions);
            foreach (SelectedConsumablesData selectedConsumablesData in consumablesData)
            {
                string itemId = selectedConsumablesData.ItemId;
                Item item = _itemFactory.Create(itemId);
                hero.Inventory.AddConsumable(item);
            }
            return hero;
        }
    }
}