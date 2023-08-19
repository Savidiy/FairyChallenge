using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Fight
{
    public class FightAutoTests : MonoBehaviour
    {
        public AttackLibrary AttackLibrary;
        public HeroLibrary HeroLibrary;
        public FightTestLibrary FightTestLibrary;
        public FightSettings FightSettings;
        public List<FightTestId> FightTestIds = new();

        private HeroFactory _heroFactory;
        private FightCalculator _fightCalculator;
        private readonly AttackResultLogger _attackResultLogger = new();

        [Button]
        public void Run()
        {
            Initialize();

            foreach (FightTestId fightTestId in FightTestIds)
            {
                FightTestStaticData testStaticData = FightTestLibrary.GetFightTest(fightTestId.TestId);
                HeroTestData ourHeroData = testStaticData.OurHero;
                Hero hero = _heroFactory.Create(ourHeroData.HeroId, ourHeroData.Level);
                for (var index = 0; index < testStaticData.Enemies.Count; index++)
                {
                    HeroTestData heroTestData = testStaticData.Enemies[index];
                    Hero enemy = _heroFactory.Create(heroTestData.HeroId, heroTestData.Level);
                    float progress = (float) index / testStaticData.Enemies.Count;
                    DisplayProgressBar("Fight", $"{hero} vs {enemy}", progress);
                    CalcFight(hero, enemy);
                }
            }

            ClearProgressBar();
        }

        private static void ClearProgressBar()
        {
#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
        }

        private static void DisplayProgressBar(string title, string info, float progress)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayProgressBar(title, info, progress);
#endif
        }

        private void CalcFight(Hero hero, Hero enemy)
        {
            Debug.Log($"{hero}: {hero.PrintStats()} vs {enemy}: {enemy.PrintStats()}\n");

            List<Hero> heroes = new List<Hero> {hero, enemy};

            int turn = 1;

            while (hero.IsAlive && enemy.IsAlive)
            {
                int heroAttackIndex = 0;
                int enemyAttackIndex = 0;

                bool heroHasMoreSpeed = hero.Stats.Get(StatType.Speed) > enemy.Stats.Get(StatType.Speed);
                Hero firstHero = heroHasMoreSpeed ? hero : enemy;
                int firstHeroAttackIndex = heroHasMoreSpeed ? heroAttackIndex : enemyAttackIndex;
                Hero secondHero = heroHasMoreSpeed ? enemy : hero;
                int secondHeroAttackIndex = heroHasMoreSpeed ? enemyAttackIndex : heroAttackIndex;
                
                _attackResultLogger.SaveState(heroes);
                AttackResult attackResult = _fightCalculator.CalcAttack(firstHero, firstHeroAttackIndex, secondHero);
                ApplyResult(attackResult, heroes);
                _attackResultLogger.ApplyState(heroes);
                Debug.Log($"{turn}: {firstHero} attacks {secondHero}: {_attackResultLogger.PrintLog()}");

                if (secondHero.IsAlive)
                {
                    _attackResultLogger.SaveState(heroes);
                    attackResult = _fightCalculator.CalcAttack(secondHero, secondHeroAttackIndex, firstHero);
                    ApplyResult(attackResult, heroes);
                    _attackResultLogger.ApplyState(heroes);
                    Debug.Log($"{turn}: {secondHero} attacks {firstHero}: {_attackResultLogger.PrintLog()}");
                }

                turn++;
            }

            Debug.Log($"Result {hero} {PrintResult(hero)} vs {enemy} {PrintResult(enemy)}\n");
        }

        private string PrintResult(Hero hero)
        {
            return hero.IsAlive ? "<color=green>is alive</color>" : "<color=red>is dead</color>";
        }

        private static void ApplyResult(AttackResult attackResult, List<Hero> heroes)
        {
            foreach (Hero hero in heroes)
            {
                foreach (StatChangeData heroChange in attackResult.GetChanges(hero))
                {
                    hero.Stats.ApplyChange(heroChange);
                }
            }
        }

        private void Initialize()
        {
            var attackFactory = new AttackFactory(AttackLibrary);
            _heroFactory = new HeroFactory(HeroLibrary, attackFactory);
            _fightCalculator = new FightCalculator(FightSettings);
        }
    }
}