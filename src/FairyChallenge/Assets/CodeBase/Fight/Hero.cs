using System.Collections.Generic;
using Savidiy.Utils;

namespace Fight
{
    public class Hero
    {
        private readonly HeroStaticData _heroStaticData;
        private readonly List<AttackData> _availableAttacks = new();

        public IReadOnlyList<AttackData> AvailableAttacks => _availableAttacks;
        public bool IsAlive => Stats.IsAlive;
        public readonly HeroStats Stats;

        public Hero(HeroStaticData heroStaticData, IReadOnlyList<AttackData> allAttacks)
        {
            _heroStaticData = heroStaticData;
            _availableAttacks.AddRange(allAttacks);
            Stats = new HeroStats(_heroStaticData);
        }

        public override string ToString() => _heroStaticData.HeroId;
        public string ForConsole => _heroStaticData.HeroId.Color(_heroStaticData.ConsoleColor);

        public string PrintStats()
        {
            return Stats.ToString();
        }
    }
}