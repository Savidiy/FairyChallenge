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
                TestIds.Add(test.TestId);
        }
    }

    [Serializable]
    public class FightTestStaticData
    {
        public string TestId;
        public HeroTestData OurHero;

        [ListDrawerSettings(AlwaysAddDefaultValue = true)]
        public List<HeroTestData> Enemies = new();
    }

    [Serializable]
    public class HeroTestData
    {
        [ValueDropdown(nameof(HeroIds)), HorizontalGroup] public string HeroId;
        private ValueDropdownList<string> HeroIds => OdinHeroIdProvider.HeroIds;
        [HorizontalGroup] public int Level;
    }
}