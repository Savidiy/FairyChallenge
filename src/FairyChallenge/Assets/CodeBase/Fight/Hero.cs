using System.Collections.Generic;

namespace Fight
{
    public class Hero
    {
        private readonly HeroStaticData _heroStaticData;
        private readonly List<AttackData> _availableAttacks = new();

        public int Level;
        private readonly IReadOnlyList<AttackData> _allAttacks;
        public IReadOnlyList<AttackData> AvailableAttacks => _availableAttacks;
        public bool IsAlive => Stats.IsAlive;
        public readonly HeroStats Stats = new();

        public Hero(HeroStaticData heroStaticData, int level, IReadOnlyList<AttackData> allAttacks)
        {
            _heroStaticData = heroStaticData;
            Level = level;
            _allAttacks = allAttacks;
            FillAvailableAttacks(Level);
            UpdateStats(Level, Stats);
        }

        private void UpdateStats(int level, HeroStats heroStats)
        {
            heroStats.Init(_heroStaticData.Stats, level);
        }

        private void FillAvailableAttacks(int level)
        {
            _availableAttacks.Clear();
            foreach (AttackData attack in _allAttacks)
            {
                if (attack.FromLevel > level)
                    continue;

                _availableAttacks.Add(attack);
            }
        }

        public override string ToString()
        {
            return $"{_heroStaticData.HeroId}({Level})";
        }

        public string PrintStats()
        {
            return Stats.ToString();
        }
    }
}