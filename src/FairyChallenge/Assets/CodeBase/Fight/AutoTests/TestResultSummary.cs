using System.Collections.Generic;

namespace Fight
{
    internal class TestResultSummary
    {
        public readonly Dictionary<StatType, int> Stats = new();
        public readonly bool HeroIsAlive;
        public readonly bool EnemyIsAlive;
        public readonly int LastTurn;
        private readonly string _printCurrentVariant;
        public int Count { get; private set; } = 1;

        public TestResultSummary(Hero hero, Hero enemy, int lastTurn, string printCurrentVariant)
        {
            _printCurrentVariant = printCurrentVariant;
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

            foreach ((StatType statType, int value) in Stats)
                if (value != hero.Stats.Get(statType))
                    return false;

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
                $"Hero {FightAutoTests.PrintAlive(HeroIsAlive)}, Enemy {FightAutoTests.PrintAlive(EnemyIsAlive)}, LastTurn: {LastTurn}, HP={Stats[StatType.CurrentHealthPoints]}/{Stats[StatType.MaxHealthPoints]}, Att={Stats[StatType.Attack]}, Def={Stats[StatType.Defence]}, {percent}% {Count}/{sum} ex. {_printCurrentVariant}";
        }
    }
}