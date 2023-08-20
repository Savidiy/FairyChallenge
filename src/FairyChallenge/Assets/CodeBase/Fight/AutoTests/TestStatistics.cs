using System.Collections.Generic;
using System.Linq;
using Savidiy.Utils;
using UnityEngine;

namespace Fight
{
    internal class TestStatistics
    {
        private readonly FightTestLibrary _fightTestLibrary;
        private readonly List<TestResultSummary> _results = new();

        public TestStatistics(FightTestLibrary fightTestLibrary)
        {
            _fightTestLibrary = fightTestLibrary;
        }

        public void SaveLog(string testId)
        {
            _results.Sort(Comparison);
            int sum = _results.Select(a => a.Count).Sum();
            string result = string.Join("\n", _results.Select(a => a.PrintLog(sum)));
            Debug.Log($"Test '{testId.Color("white")}' results:\n{result}");
            _fightTestLibrary.SaveTestResult(testId, result);
            _results.Clear();
        }

        private int Comparison(TestResultSummary x, TestResultSummary y)
        {
            int compareTo = x.HeroIsAlive.CompareTo(y.HeroIsAlive);
            if (compareTo != 0)
                return compareTo;
            
            compareTo = x.EnemyIsAlive.CompareTo(y.EnemyIsAlive);
            if (compareTo != 0)
                return compareTo;
            
            compareTo = -x.Stats[StatType.CurrentHealthPoints].CompareTo(y.Stats[StatType.CurrentHealthPoints]);
            if (compareTo != 0)
                return compareTo;
            
            compareTo = x.LastTurn.CompareTo(y.LastTurn);
            if (compareTo != 0)
                return compareTo;
            
            compareTo = -x.Count.CompareTo(y.Count);
            if (compareTo != 0)
                return compareTo;
            
            return 0;
        }

        public void Add(Hero hero, Hero enemy, int lastTurn, string printCurrentVariant)
        {
            foreach (TestResultSummary testResultSummary in _results)
            {
                if (testResultSummary.IsSame(hero, enemy, lastTurn))
                {
                    testResultSummary.IncreaseCount();
                    return;
                }
            }
            
            _results.Add(new TestResultSummary(hero, enemy, lastTurn, printCurrentVariant));
        }
    }
}