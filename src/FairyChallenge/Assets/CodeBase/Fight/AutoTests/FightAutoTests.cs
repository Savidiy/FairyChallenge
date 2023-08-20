using System.Collections.Generic;
using Savidiy.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using static Savidiy.Utils.ConsoleColor;
using static Savidiy.Utils.SafeEditorUtils;

namespace Fight
{
    public class FightAutoTests : MonoBehaviour
    {
        private const string DEPENDENCIES = "Dependencies";
        private const string BUTTONS = "Buttons";
        [FormerlySerializedAs("AttackLibrary")] [FoldoutGroup(DEPENDENCIES)] public ActionLibrary ActionLibrary;
        [FoldoutGroup(DEPENDENCIES)] public HeroLibrary HeroLibrary;
        [FoldoutGroup(DEPENDENCIES)] public FightTestLibrary FightTestLibrary;
        [FoldoutGroup(DEPENDENCIES)] public FightSettings FightSettings;

        public bool ShowVariantsLog;
        public List<FightTestId> FightTestIds = new();

        private HeroFactory _heroFactory;
        private FightCalculator _fightCalculator;
        private TestStatistics _testStatistics;
        private readonly ActionResultLogger _actionResultLogger = new();

        private void Initialize()
        {
            var actionFactory = new ActionFactory(ActionLibrary);
            _heroFactory = new HeroFactory(HeroLibrary, actionFactory);
            _fightCalculator = new FightCalculator(FightSettings);
            _testStatistics = new TestStatistics(FightTestLibrary);
        }

        [Button, HorizontalGroup(BUTTONS)]
        public void Run()
        {
            List<FightTestId> fightTestIds = FightTestIds;
            StartTests(fightTestIds, ShowVariantsLog);
        }

        public void StartTests(List<FightTestId> fightTestIds, bool needDetails)
        {
            Initialize();

            foreach (FightTestId fightTestId in fightTestIds)
            {
                string testId = fightTestId.TestId;
                TestFight(testId, needDetails);
                _testStatistics.SaveLog(testId);
            }

            ClearProgressBar();
        }

        [Button, HorizontalGroup(BUTTONS)]
        public void ClearConsole() => ClearLogConsole();

        private void TestFight(string testId, bool needDetails)
        {
            FightTestStaticData data = FightTestLibrary.GetFightTest(testId);

            Hero hero = CreateHero(data);
            Hero enemy = CreateEnemy(data);
            Debug.Log($"Start test '{testId}' {hero.ForConsole} {hero.PrintStats()} VS {enemy.ForConsole}: {enemy.PrintStats()}");

            var actionIterator = new ActionIterator(hero, enemy);
            do
            {
                hero = CreateHero(data);
                enemy = CreateEnemy(data);
                int currentVariantNumber = actionIterator.CurrentVariantNumber();
                int estimateVariantsCount = actionIterator.EstimateVariantsCount();
                float progress = currentVariantNumber / (float) estimateVariantsCount;
                var info = $"{hero} vs {enemy}\nVariant {currentVariantNumber}/{estimateVariantsCount}";
                DisplayProgressBar("Fight", info, progress);

                CalcFight(hero, enemy, actionIterator, needDetails);
            } while (actionIterator.HasNext());
        }

        private Hero CreateHero(FightTestStaticData data)
        {
            return _heroFactory.Create(data.HeroId, data.AdditionalActions);
        }

        private Hero CreateEnemy(FightTestStaticData data)
        {
            return _heroFactory.Create(data.EnemyId);
        }

        private void CalcFight(Hero hero, Hero enemy, ActionIterator actionIterator, bool needDetails)
        {
            var log = string.Empty;
            var heroes = new List<Hero> {hero, enemy};
            var turn = 0;
            var turnString = string.Empty;

            while (hero.IsAlive && enemy.IsAlive)
            {
                actionIterator.GetIndexForTurn(turn, out int firstIndex, out int secondIndex);

                _actionResultLogger.SaveState(heroes);
                ActionResult actionResult = _fightCalculator.CalcAction(hero, firstIndex, enemy);
                ApplyResult(actionResult, heroes);
                _actionResultLogger.ApplyState(heroes);
                turnString = (turn + 1).ToString();
                log += LogTurn(turnString, hero, enemy, actionResult, _actionResultLogger);

                if (enemy.IsAlive)
                {
                    _actionResultLogger.SaveState(heroes);
                    actionResult = _fightCalculator.CalcAction(enemy, secondIndex, hero);
                    ApplyResult(actionResult, heroes);
                    _actionResultLogger.ApplyState(heroes);
                    log += LogTurn(turnString, enemy, hero, actionResult, _actionResultLogger);
                }

                turn++;
            }

            string printCurrentVariant = actionIterator.PrintCurrentVariant();
            if (needDetails)
                Debug.Log(
                    $"Result {hero.ForConsole} {PrintAlive(hero.IsAlive)} vs {enemy.ForConsole} {PrintAlive(enemy.IsAlive)} at turn {turnString}: {printCurrentVariant}\n{log}");

            int lastTurn = turn - 1;
            _testStatistics.Add(hero, enemy, lastTurn, printCurrentVariant);
            actionIterator.FightEndOnTurn(lastTurn);
        }

        private string LogTurn(string logTurn, Hero firstHero, Hero secondHero, ActionResult actionResult,
            ActionResultLogger actionResultLogger)
        {
            return
                $"{logTurn}: {firstHero.ForConsole} use {actionResult.ActionId.Color(WHITE)}: {actionResultLogger.PrintLog()}\n";
        }

        public static string PrintAlive(bool isAlive)
        {
            return isAlive ? "is alive".Color(GREEN) : "is dead".Color(RED);
        }

        private static void ApplyResult(ActionResult actionResult, List<Hero> heroes)
        {
            foreach (Hero hero in heroes)
            foreach (StatChangeData heroChange in actionResult.GetChanges(hero))
                hero.Stats.ApplyChange(heroChange);
        }
    }
}