using System.Collections.Generic;

namespace Fight
{
    public class HeroFactory
    {
        private readonly HeroLibrary _heroLibrary;
        private readonly ActionFactory _actionFactory;

        public HeroFactory(HeroLibrary heroLibrary, ActionFactory actionFactory)
        {
            _heroLibrary = heroLibrary;
            _actionFactory = actionFactory;
        }

        public Hero Create(string heroId)
        {
            var heroData = _heroLibrary.GetStaticData(heroId);
            IReadOnlyList<ActionData> actions = _actionFactory.Create(heroData.Actions);
            var hero = new Hero(heroData, actions);
            return hero;
        }

        public Hero Create(HeroTestData enemyTestData)
        {
            string heroId = enemyTestData.HeroId;
            var heroData = _heroLibrary.GetStaticData(heroId);
            List<ActionData> actions = _actionFactory.Create(heroData.Actions);
            foreach (AdditionalActionData additionalActionData in enemyTestData.AdditionalActions)
                actions.Add(_actionFactory.Create(additionalActionData.ActionId));

            var hero = new Hero(heroData, actions);
            return hero;
        }
    }
}