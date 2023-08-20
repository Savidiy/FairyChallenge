using System;
using System.Collections.Generic;
using Savidiy.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fight
{
    [CreateAssetMenu(fileName = nameof(FightTestLibrary), menuName = nameof(FightTestLibrary), order = 0)]
    public class FightTestLibrary : AutoSaveScriptableObject
    {
        [ListDrawerSettings(ListElementLabelName = "@this")]
        public List<FightTestStaticData> Tests = new();

        public ValueDropdownList<string> TestIds = new();

        public FightTestStaticData GetFightTest(string testId)
        {
            foreach (FightTestStaticData fightTest in Tests)
                if (fightTest.TestId.Equals(testId))
                    return fightTest;

            Debug.LogError($"Fight test with id '{testId}' not found");
            return new FightTestStaticData() {TestId = testId};
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            TestIds.Clear();
            foreach (FightTestStaticData test in Tests)
            {
                HeroTestData hero = test.OurHero;
                HeroTestData enemy = test.Enemy;
                test.TestId =
                    $"{hero.HeroId} {hero.Level}{(hero.AdditionalAttacks.Count > 0 ? $"({string.Join(",", hero.AdditionalAttacks)})" : "")} vs {enemy.HeroId} {enemy.Level}{(enemy.AdditionalAttacks.Count > 0 ? $"({string.Join(",", enemy.AdditionalAttacks)})" : "")}";

                TestIds.Add(test.TestId);
            }

            SavePrefab();
        }

        public void SaveTestResult(string testId, string result)
        {
            FightTestStaticData fightTestStaticData = GetFightTest(testId);
            fightTestStaticData.LastResult = result;
            SavePrefab();
        }
    }

    [Serializable]
    public class FightTestStaticData
    {
        public string TestId;
        public HeroTestData OurHero;
        public HeroTestData Enemy;
        [ShowInInspector] public string LastResult;
        [Button] private void ToConsole() => Debug.Log($"Test '{TestId.Color("white")}' results:\n{LastResult}");

        public override string ToString() => TestId;
    }

    [Serializable]
    public class HeroTestData
    {
        [ValueDropdown(nameof(HeroIds)), HorizontalGroup] public string HeroId;
        private ValueDropdownList<string> HeroIds => OdinHeroIdProvider.HeroIds;
        [HorizontalGroup] public int Level;

        public List<AdditionalAttackData> AdditionalAttacks = new();
    }

    [Serializable]
    public class AdditionalAttackData
    {
        [ValueDropdown(nameof(AttackIds)), HideLabel] public string AttackId;
        private ValueDropdownList<string> AttackIds => OdinAttackIdProvider.AttackIds;

        public override string ToString()
        {
            return AttackId;
        }
    }
}