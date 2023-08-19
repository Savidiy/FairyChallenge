﻿using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Fight
{
    public class FightAutoTests : MonoBehaviour
    {
        private const string DEPENDENCIES = "Dependencies";
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

        [Button]
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

        private void CalcFight(Hero hero, Hero enemy, AttackIterator attackIterator)
        {
            var log = string.Empty;
            var heroes = new List<Hero> {hero, enemy};
            var turn = 0;
            var turnString = string.Empty;

            while (hero.IsAlive && enemy.IsAlive)
            {
                int heroAttackIndex;
                int enemyAttackIndex;
                attackIterator.GetIndexForTurn(turn, out heroAttackIndex, out enemyAttackIndex);

                bool heroHasMoreSpeed = hero.Stats.Get(StatType.Speed) > enemy.Stats.Get(StatType.Speed);
                Hero firstHero = heroHasMoreSpeed ? hero : enemy;
                int firstHeroAttackIndex = heroHasMoreSpeed ? heroAttackIndex : enemyAttackIndex;
                Hero secondHero = heroHasMoreSpeed ? enemy : hero;
                int secondHeroAttackIndex = heroHasMoreSpeed ? enemyAttackIndex : heroAttackIndex;

                _attackResultLogger.SaveState(heroes);
                AttackResult attackResult = _fightCalculator.CalcAttack(firstHero, firstHeroAttackIndex, secondHero);
                ApplyResult(attackResult, heroes);
                _attackResultLogger.ApplyState(heroes);
                turnString = (turn + 1).ToString();
                log += LogTurn(turnString, firstHero, secondHero, attackResult, _attackResultLogger);

                if (secondHero.IsAlive)
                {
                    _attackResultLogger.SaveState(heroes);
                    attackResult = _fightCalculator.CalcAttack(secondHero, secondHeroAttackIndex, firstHero);
                    ApplyResult(attackResult, heroes);
                    _attackResultLogger.ApplyState(heroes);
                    log += LogTurn(turnString, secondHero, firstHero, attackResult, _attackResultLogger);
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