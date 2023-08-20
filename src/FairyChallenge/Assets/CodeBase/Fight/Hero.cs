using Savidiy.Utils;

namespace Fairy
{
    public class Hero
    {
        public HeroStaticData StaticData { get; }

        public bool IsAlive => Stats.IsAlive;
        public readonly HeroStats Stats;
        public readonly Inventory Inventory;
        public readonly HeroActions HeroActions;
        
        public Hero(HeroStaticData heroStaticData, HeroActions heroActions, HeroStats heroStats, Inventory inventory)
        {
            StaticData = heroStaticData;
            HeroActions = heroActions;
            Stats = heroStats;
            Inventory = inventory;
        }

        public override string ToString() => StaticData.HeroId;
        public string ForConsole => StaticData.HeroId.Color(StaticData.ConsoleColor);

        public string PrintStats()
        {
            return Stats.ToString();
        }
    }
}