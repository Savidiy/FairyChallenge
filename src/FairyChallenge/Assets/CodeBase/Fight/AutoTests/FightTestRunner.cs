using System.Collections.Generic;
using UnityEngine;

namespace Fight
{
    public static class FightTestRunner
    {
        private static FightAutoTests _fightAutoTests;

        public static void StartTest(string testId, bool needDetails)
        {
            if (_fightAutoTests == null)
                _fightAutoTests = Object.FindObjectOfType<FightAutoTests>();
            
            if (_fightAutoTests == null)
            {
                Debug.LogError("FightAutoTests not found");
                return;
            }

            _fightAutoTests.StartTests(new List<FightTestId>() {new() {TestId = testId}}, needDetails);
        }
    }
}