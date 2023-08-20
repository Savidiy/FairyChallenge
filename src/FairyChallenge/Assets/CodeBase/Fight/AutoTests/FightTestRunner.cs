using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fight
{
    public static class FightTestRunner
    {
        private static FightAutoTests _fightAutoTests;

        public static void StartTest(string testId, bool needDetails)
        {
            if (IsTesterValid())
                return;

            _fightAutoTests.StartTests(new List<FightTestId>() {new() {TestId = testId}}, needDetails);
        }

        public static void StartTest(string testId, string actionVariants)
        {
            if (IsTesterValid())
                return;

            string[] split = actionVariants.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string actionVariant in split)
                _fightAutoTests.StartTest(testId, actionVariant);
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