using Savidiy.Utils;

namespace Fight
{
    public class Hero
    {
        private readonly HeroStaticData _heroStaticData;
        
        public bool IsAlive => Stats.IsAlive;
        public readonly HeroStats Stats;
        public readonly Inventory Inventory;
        public readonly HeroActions HeroActions;
        
        public Hero(HeroStaticData heroStaticData, HeroActions heroActions, HeroStats heroStats, Inventory inventory)
        {
            _heroStaticData = heroStaticData;
            HeroActions = heroActions;
            Stats = heroStats;
            Inventory = inventory;
        }

        public override string ToString() => _heroStaticData.HeroId;
        public string ForConsole => _heroStaticData.HeroId.Color(_heroStaticData.ConsoleColor);

        public string PrintStats()
        {
            return Stats.ToString();
        }
    }
}