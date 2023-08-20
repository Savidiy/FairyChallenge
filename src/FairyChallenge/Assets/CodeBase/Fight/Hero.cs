using System.Collections.Generic;
using Savidiy.Utils;

namespace Fight
{
    public class Hero
    {
        private readonly HeroStaticData _heroStaticData;
        private readonly List<ActionData> _actions = new();

        public IReadOnlyList<ActionData> Actions => _actions;
        public bool IsAlive => Stats.IsAlive;
        public readonly HeroStats Stats;
        public readonly Inventory Inventory = new();

        public Hero(HeroStaticData heroStaticData, IReadOnlyList<ActionData> allAttacks)
        {
            _heroStaticData = heroStaticData;
            _actions.AddRange(allAttacks);
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