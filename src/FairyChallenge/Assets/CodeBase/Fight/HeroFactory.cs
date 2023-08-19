using System.Collections.Generic;

namespace Fight
{
    public class HeroFactory
    {
        private readonly HeroLibrary _heroLibrary;
        private readonly AttackFactory _attackFactory;

        public HeroFactory(HeroLibrary heroLibrary, AttackFactory attackFactory)
        {
            _heroLibrary = heroLibrary;
            _attackFactory = attackFactory;
        }

        public Hero Create(string heroId, int level)
        {
            var heroData = _heroLibrary.GetStaticData(heroId);
            IReadOnlyList<AttackData> attacks = _attackFactory.Create(heroData.Attacks);
            var hero = new Hero(heroData, level, attacks);
            return hero;
        }

        public Hero Create(HeroTestData enemyTestData)
        {
            string heroId = enemyTestData.HeroId;
            int level = enemyTestData.Level;
            var heroData = _heroLibrary.GetStaticData(heroId);
            List<AttackData> attacks = _attackFactory.Create(heroData.Attacks);
            foreach (AdditionalAttackData additionalAttackData in enemyTestData.AdditionalAttacks)
                attacks.Add(_attackFactory.Create(additionalAttackData.AttackId, 0));

            var hero = new Hero(heroData, level, attacks);
            return hero;
        }
    }
}