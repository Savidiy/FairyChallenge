using System;
using System.Collections.Generic;
using Savidiy.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fight
{
    [CreateAssetMenu(fileName = nameof(FightTestLibrary), menuName = nameof(FightTestLibrary), order = 0)]
    public class FightTestLibrary : AutoSaveScriptableObject
    {
        [ListDrawerSettings(ListElementLabelName = "@this")]
        public List<FightTestStaticData> Tests = new();

        public readonly ValueDropdownList<string> TestIds = new();

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
                string heroId = test.HeroId;
                string enemyId = test.EnemyId;
                List<AdditionalActionData> actions = test.AdditionalActions;
                test.TestId =
                    $"{heroId} {(actions.Count > 0 ? $"({string.Join(",", actions)}) " : "")}vs {enemyId}";

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
        public string TestId = string.Empty;
        public List<AdditionalActionData> AdditionalActions = new();
        [ValueDropdown(nameof(HeroIds))] public string HeroId;
        private ValueDropdownList<string> HeroIds => OdinHeroIdProvider.HeroIds;
        [ValueDropdown(nameof(HeroIds))] public string EnemyId;
        [ShowInInspector] public string LastResult;
        [Button, HorizontalGroup(width:0.2f)] private void ToConsole() => Debug.Log($"Test '{TestId.Color("white")}' results:\n{LastResult}");
        [Button, HorizontalGroup] private void Test() => FightTestRunner.StartTest(TestId, false);
        [Button, HorizontalGroup] private void TestWithDetails() => FightTestRunner.StartTest(TestId, true);
        [Button, HorizontalGroup(width:0.2f)] private void ClearConsole() => SafeEditorUtils.ClearLogConsole();

        public override string ToString() => TestId;
    }

    [Serializable]
    public class AdditionalActionData
    {
        [FormerlySerializedAs("AttackId")] [ValueDropdown(nameof(ActionIds)), HideLabel] public string ActionId;
        private ValueDropdownList<string> ActionIds => OdinActionIdProvider.ActionIds;

        public override string ToString()
        {
            return ActionId;
        }
    }
}