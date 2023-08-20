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
        [FoldoutGroup(DEPENDENCIES)] public ItemsLibrary ItemsLibrary;
        [FoldoutGroup(DEPENDENCIES)] public FightSettings FightSettings;

        public bool ShowVariantsLog;
        public List<FightTestId> FightTestIds = new();

        private HeroFactory _heroFactory;
        private FightCalculator _fightCalculator;
        private TestStatistics _testStatistics;
        private readonly ActionResultLogger _actionResultLogger = new();
        private ItemFactory _itemFactory;

        private void Initialize()
        {
            var actionFactory = new ActionFactory(ActionLibrary);
            _itemFactory = new ItemFactory(ItemsLibrary);
            _heroFactory = new HeroFactory(HeroLibrary, actionFactory, _itemFactory);
            _fightCalculator = new FightCalculator(FightSettings);
            _testStatistics = new TestStatistics(FightTestLibrary);
        }

        [Button, HorizontalGroup(BUTTONS)]
        public void Run()
        {
            List<FightTestId> fightTestIds = FightTestIds;
            List<FightTestStaticData> datas = fightTestIds.ConvertAll(id => FightTestLibrary.GetFightTest(id.TestId));
            StartTests(datas, ShowVariantsLog);
        }

        [Button, HorizontalGroup(BUTTONS)]
        public void ClearConsole() => ClearLogConsole();

        public void StartTests(List<FightTestStaticData> fightTestIds, bool needDetails)
        {
            Initialize();

            foreach (FightTestStaticData data in fightTestIds)
            {
                TestFight(needDetails, data);
                _testStatistics.SaveLog(data.TestId);
            }

            ClearProgressBar();
        }

        public void StartTest(FightTestStaticData data, string actionVariant)
        {
            Initialize();

            Hero hero = CreateHero(data);
            Hero enemy = CreateEnemy(data);
            Debug.Log($"Start test '{data.TestId}' {hero.ForConsole} {hero.PrintStats()} VS {enemy.ForConsole}: {enemy.PrintStats()}");

            var actionIterator = new ActionIterator(hero, enemy, actionVariant);
            CalcFight(hero, enemy, actionIterator, needDetails: true, maxTurns: FightSettings.MaxTurns);
        }

        private void TestFight(bool needDetails, FightTestStaticData data)
        {
            var testId = data.TestId;
            Hero hero = CreateHero(data);
            Hero enemy = CreateEnemy(data);
            Debug.Log($"Start test '{testId}' {hero.ForConsole} {hero.PrintStats()} VS {enemy.ForConsole}: {enemy.PrintStats()}");

            var actionIterator = new ActionIterator(hero, enemy);
            do
            {
                hero = CreateHero(data);
                enemy = CreateEnemy(data);
                long currentVariantNumber = actionIterator.CurrentVariantNumber();
                double estimateVariantsCount = actionIterator.EstimateVariantsCount();
                var progress = (float) (currentVariantNumber / estimateVariantsCount);
                var info = $"{hero} vs {enemy}\nVariant {currentVariantNumber}/{estimateVariantsCount:F0}";
                DisplayProgressBar("Fight", info, progress);

                CalcFight(hero, enemy, actionIterator, needDetails, data.MaxTurns);
            } while (actionIterator.HasNext());
        }

        private Hero CreateHero(FightTestStaticData data)
        {
            Hero hero = _heroFactory.Create(data.HeroId, data.AdditionalActions, data.Consumables);
            if (data.Accessory.HasItem())
                hero.Inventory.UsedAccessory.SetItem(_itemFactory.Create(data.Accessory.ItemId));
            
            if (data.Weapon.HasItem())
                hero.Inventory.UsedWeapon.SetItem(_itemFactory.Create(data.Weapon.ItemId));
            
            if (data.Armor.HasItem())
                hero.Inventory.UsedArmor.SetItem(_itemFactory.Create(data.Armor.ItemId));

            return hero;
        }

        private Hero CreateEnemy(FightTestStaticData data)
        {
            return _heroFactory.Create(data.EnemyId);
        }

        private void CalcFight(Hero hero, Hero enemy, ActionIterator actionIterator, bool needDetails, int maxTurns)
        {
            var log = string.Empty;
            var heroes = new List<Hero> {hero, enemy};
            var turn = 0;
            var turnString = string.Empty;
            var startStatLog = $"Start stat {hero.ForConsole} {hero.PrintStats()} VS {enemy.ForConsole} {enemy.PrintStats()}";

            while (hero.IsAlive && enemy.IsAlive && turn < maxTurns)
            {
                actionIterator.GetIndexForTurn(turn, out int firstIndex, out int secondIndex);

                _actionResultLogger.SaveState(heroes);
                if (!_fightCalculator.TryCalcResult(hero, firstIndex, enemy, out var actionResult))
                {   
                    actionIterator.FightEndOnTurn(turn);
                    return;
                }

                ApplyResult(actionResult, heroes);
                _actionResultLogger.ApplyState(heroes);
                turnString = (turn + 1).ToString();
                log += LogTurn(turnString, hero, enemy, actionResult, _actionResultLogger);

                if (enemy.IsAlive)
                {
                    _actionResultLogger.SaveState(heroes);
                    if (!_fightCalculator.TryCalcResult(enemy, secondIndex, hero, out actionResult))
                    {
                        actionIterator.FightEndOnTurn(turn);
                        return;
                    }

                    ApplyResult(actionResult, heroes);
                    _actionResultLogger.ApplyState(heroes);
                    log += LogTurn(turnString, enemy, hero, actionResult, _actionResultLogger);
                }

                turn++;
            }

            string printCurrentVariant = actionIterator.PrintCurrentVariant();
            if (needDetails)
            {
                var endStatLog = $"End stat {hero.ForConsole} {hero.PrintStats()} VS {enemy.ForConsole} {enemy.PrintStats()}";
                Debug.Log(
                    $"Result {hero.ForConsole} {PrintAlive(hero.IsAlive)} vs {enemy.ForConsole} {PrintAlive(enemy.IsAlive)} at turn {turnString}: {printCurrentVariant}\n" +
                    $"{log}\n{startStatLog}\n{endStatLog}");
            }

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