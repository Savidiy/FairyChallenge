using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fairy
{
    public static class FightTestRunner
    {
        private static FightAutoTests _fightAutoTests;

        public static void StartTest(FightTestStaticData data, bool needDetails)
        {
            if (IsTesterValid())
                return;

            _fightAutoTests.StartTests(new List<FightTestStaticData>() {data}, needDetails);
        }

        public static void StartTest(FightTestStaticData data, string actionVariants)
        {
            if (IsTesterValid())
                return;

            string[] split = actionVariants.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string actionVariant in split)
                _fightAutoTests.StartTest(data, actionVariant);
        }

        private static bool IsTesterValid()
        {
            if (_fightAutoTests == null)
                _fightAutoTests = Object.FindObjectOfType<FightAutoTests>();

            if (_fightAutoTests == null)
            {
                Debug.LogError("FightAutoTests not found");
                return true;
            }

            return false;
        }
    }
}