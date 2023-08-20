using System;
using System.Collections.Generic;
using System.Linq;
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
                List<SelectedConsumablesData> consumables = test.Consumables;
                test.TestId =
                    $"{heroId} (" +
                    $"{(test.Weapon.HasItem() ? $"W:{test.Weapon.ItemId}" : "")} " +
                    $"{(test.Armor.HasItem() ? $"S:{test.Armor.ItemId}" : "")} " +
                    $"{(test.Accessory.HasItem() ? $"A:{test.Accessory.ItemId}" : "")} " +
                    $"{(actions.Count > 0 ? $"{string.Join(",", actions)}" : "")} " +
                    $"{(consumables.Count > 0 ? $"C:{string.Join(",", consumables.Select(a => a.ItemId))}" : "")}" +
                    $") vs {enemyId}";

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
        public List<SelectedConsumablesData> Consumables = new();
        [LabelText("@" + nameof(Weapon))] public SelectedWeaponData Weapon;
        [LabelText("@" + nameof(Armor))] public SelectedArmorData Armor;
        [LabelText("@" + nameof(Accessory))] public SelectedAccessoryData Accessory;
        [ValueDropdown(nameof(HeroIds))] public string HeroId;
        private ValueDropdownList<string> HeroIds => OdinHeroIdProvider.HeroIds;
        [ValueDropdown(nameof(HeroIds))] public string EnemyId;
        public int MaxTurns = 8;
        [ShowInInspector] public string LastResult;

        [Button, HorizontalGroup(width: 0.2f)] private void ToConsole() =>
            Debug.Log($"Test '{TestId.Color("white")}' results:\n{LastResult}");

        [Button, HorizontalGroup] private void Test() => FightTestRunner.StartTest(this, false);
        [Button, HorizontalGroup] private void TestWithDetails() => FightTestRunner.StartTest(this, true);
        [Button, HorizontalGroup(width: 0.2f)] private void ClearConsole() => SafeEditorUtils.ClearLogConsole();

        [InlineButton(nameof(TestVariant))] public string Variant;
        private void TestVariant() => FightTestRunner.StartTest(this, Variant);

        public override string ToString() => TestId;
    }

    [Serializable]
    public class AdditionalActionData
    {
        [FormerlySerializedAs("AttackId")] [ValueDropdown(nameof(ActionIds)), HideLabel] public string ActionId;
        private ValueDropdownList<string> ActionIds => OdinActionIdProvider.ActionIds;

        public override string ToString() => ActionId;
    }

    [Serializable]
    public class SelectedConsumablesData : SelectedItemData
    {
        protected override ItemType ItemType => ItemType.Consumable;
    }

    [Serializable]
    public class SelectedWeaponData : SelectedItemData
    {
        protected override ItemType ItemType => ItemType.Weapon;
    }

    [Serializable]
    public class SelectedArmorData : SelectedItemData
    {
        protected override ItemType ItemType => ItemType.Armor;
    }

    [Serializable]
    public class SelectedAccessoryData : SelectedItemData
    {
        protected override ItemType ItemType => ItemType.Accessory;
    }

    [Serializable]
    public abstract class SelectedItemData
    {
        protected abstract ItemType ItemType { get; }

        [ValueDropdown(nameof(ItemIds)), HideLabel] public string ItemId;
        private ValueDropdownList<string> ItemIds => OdinItemIdProvider.GetItemIds(ItemType);

        public override string ToString() => $"{ItemType}: {ItemId}";
        public bool HasItem() => !string.IsNullOrEmpty(ItemId) && !ItemId.Equals(ItemsLibrary.NONE);
    }
}