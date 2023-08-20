using System.Collections.Generic;
using Savidiy.Utils;
using static Fairy.FightAutoTests;

namespace Fairy
{
    internal class TestResultSummary
    {
        public readonly Dictionary<StatType, int> Stats = new();
        public readonly bool HeroIsAlive;
        public readonly bool EnemyIsAlive;
        public readonly int LastTurn;
        private const int MAX_PRINT_VARIANTS = 10;
        private readonly List<string> _printCurrentVariant = new();
        private readonly Hero _hero;
        private readonly Hero _enemy;
        public int Count { get; private set; } = 1;

        public TestResultSummary(Hero hero, Hero enemy, int lastTurn, string printCurrentVariant)
        {
            _enemy = enemy;
            _hero = hero;
            _printCurrentVariant.Add(printCurrentVariant);
            LastTurn = lastTurn;
            HeroIsAlive = hero.IsAlive;
            hero.Stats.FillAllStatsValues(Stats);
            EnemyIsAlive = enemy.IsAlive;
        }

        public bool IsSame(Hero hero, Hero enemy, int lastTurn)
        {
            if (LastTurn != lastTurn)
                return false;

            if (HeroIsAlive != hero.IsAlive)
                return false;

            if (EnemyIsAlive != enemy.IsAlive)
                return false;

            if (!HeroIsAlive)
                return true;

            if (Stats[StatType.HealthPoints] == hero.Stats.Get(StatType.HealthPoints))
                return true;

            return true;
        }

        public void IncreaseCount()
        {
            Count++;
        }

        public string PrintLog(int sum)
        {
            var percent = (int) (Count * 100f / sum);
            return
                $"{_hero.ForConsole} {PrintAlive(HeroIsAlive)}, {_enemy.ForConsole} {PrintAlive(EnemyIsAlive)}, LastTurn: {LastTurn}, HP={Stats[StatType.HealthPoints].Color(ConsoleColor.RED)}/{Stats[StatType.MaxHealthPoints]}, Att={Stats[StatType.Attack]}, Def={Stats[StatType.Defence]}, {percent}% {Count}/{sum} ex. {string.Join(" ", _printCurrentVariant)}";
        }

        public void AddPrintVariant(string printCurrentVariant)
        {
            if (_printCurrentVariant.Count < MAX_PRINT_VARIANTS)
                _printCurrentVariant.Add(printCurrentVariant);
        }
    }
}