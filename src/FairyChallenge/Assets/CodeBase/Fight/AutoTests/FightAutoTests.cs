using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using static Savidiy.Utils.SafeEditorUtils;

namespace Fight
{
    public class FightAutoTests : MonoBehaviour
    {
        private const string DEPENDENCIES = "Dependencies";
        private const string BUTTONS = "Buttons";
        [FoldoutGroup(DEPENDENCIES)] public AttackLibrary AttackLibrary;
        [FoldoutGroup(DEPENDENCIES)] public HeroLibrary HeroLibrary;
        [FoldoutGroup(DEPENDENCIES)] public FightTestLibrary FightTestLibrary;
        [FoldoutGroup(DEPENDENCIES)] public FightSettings FightSettings;

        public bool ShowVariantsLog;
        public List<FightTestId> FightTestIds = new();

        private HeroFactory _heroFactory;
        private FightCalculator _fightCalculator;
        private TestStatistics _testStatistics;
        private readonly AttackResultLogger _attackResultLogger = new();

        private void Initialize()
        {
            var attackFactory = new AttackFactory(AttackLibrary);
            _heroFactory = new HeroFactory(HeroLibrary, attackFactory);
            _fightCalculator = new FightCalculator(FightSettings);
            _testStatistics = new TestStatistics(FightTestLibrary);
        }

        [Button, HorizontalGroup(BUTTONS)]
        public void Run()
        {
            Initialize();

            foreach (FightTestId fightTestId in FightTestIds)
            {
                string testId = fightTestId.TestId;
                FightTestStaticData testStaticData = FightTestLibrary.GetFightTest(testId);
                HeroTestData ourHeroData = testStaticData.OurHero;
                HeroTestData enemyTestData = testStaticData.Enemy;
                TestFight(testId, ourHeroData, enemyTestData);
                _testStatistics.SaveLog(testId);
            }

            ClearProgressBar();
        }

        [Button, HorizontalGroup(BUTTONS)]
        public void ClearConsole() => ClearLogConsole();

        private void TestFight(string testId, HeroTestData ourHeroData, HeroTestData enemyTestData)
        {
            Hero hero = _heroFactory.Create(ourHeroData);
            Hero enemy = _heroFactory.Create(enemyTestData);
            Debug.Log($"Start test '{testId}' {hero}: {hero.PrintStats()} vs {enemy}: {enemy.PrintStats()}");

            var attackIterator = new AttackIterator(hero, enemy);
            do
            {
                hero = _heroFactory.Create(ourHeroData);
                enemy = _heroFactory.Create(enemyTestData);
                int currentVariantNumber = attackIterator.CurrentVariantNumber();
                int estimateVariantsCount = attackIterator.EstimateVariantsCount();
                float progress = currentVariantNumber / (float) estimateVariantsCount;
                var info = $"{hero} vs {enemy}\nVariant {currentVariantNumber}/{estimateVariantsCount}";
                DisplayProgressBar("Fight", info, progress);

                CalcFight(hero, enemy, attackIterator);
            } while (attackIterator.HasNext());
        }

        private void CalcFight(Hero hero, Hero enemy, AttackIterator attackIterator)
        {
            var log = string.Empty;
            var heroes = new List<Hero> {hero, enemy};
            var turn = 0;
            var turnString = string.Empty;

            while (hero.IsAlive && enemy.IsAlive)
            {
                attackIterator.GetIndexForTurn(turn, out int heroAttackIndex, out int enemyAttackIndex);

                _attackResultLogger.SaveState(heroes);
                AttackResult attackResult = _fightCalculator.CalcAttack(hero, heroAttackIndex, enemy);
                ApplyResult(attackResult, heroes);
                _attackResultLogger.ApplyState(heroes);
                turnString = (turn + 1).ToString();
                log += LogTurn(turnString, hero, enemy, attackResult, _attackResultLogger);

                if (enemy.IsAlive)
                {
                    _attackResultLogger.SaveState(heroes);
                    attackResult = _fightCalculator.CalcAttack(enemy, enemyAttackIndex, hero);
                    ApplyResult(attackResult, heroes);
                    _attackResultLogger.ApplyState(heroes);
                    log += LogTurn(turnString, enemy, hero, attackResult, _attackResultLogger);
                }

                turn++;
            }

            string printCurrentVariant = attackIterator.PrintCurrentVariant();
            if (ShowVariantsLog)
                Debug.Log(
                    $"Result {hero} {PrintAlive(hero.IsAlive)} vs {enemy} {PrintAlive(enemy.IsAlive)} at turn {turnString}: {printCurrentVariant}\n{log}");

            int lastTurn = turn - 1;
            _testStatistics.Add(hero, enemy, lastTurn, printCurrentVariant);
            attackIterator.FightEndOnTurn(lastTurn);
        }

        private string LogTurn(string logTurn, Hero firstHero, Hero secondHero, AttackResult attackResult,
            AttackResultLogger attackResultLogger)
        {
            return
                $"{logTurn}: <color=white>{firstHero}</color> with <color=white>{attackResult.AttackId}</color> attacks {secondHero} : {attackResultLogger.PrintLog()}\n";
        }

        public static string PrintAlive(bool isAlive)
        {
            return isAlive ? "<color=#00FF00>is alive</color>" : "<color=red>is dead</color>";
        }

        private static void ApplyResult(AttackResult attackResult, List<Hero> heroes)
        {
            foreach (Hero hero in heroes)
            foreach (StatChangeData heroChange in attackResult.GetChanges(hero))
                hero.Stats.ApplyChange(heroChange);
        }
    }
}