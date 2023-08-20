using System.Collections.Generic;
using System.Linq;

namespace Fairy
{
    public sealed class PlayerHandler
    {
        private readonly Dictionary<StatType, int> _stats = new();
        private readonly List<Item> _consumables = new();

        public Hero Hero { get; private set; }

        public void SetHero(Hero hero)
        {
            Hero = hero;
        }

        public void SaveHeroBeforeFight()
        {
            Hero.Stats.FillAllStatsValues(_stats);
            _consumables.Clear();
            _consumables.AddRange(Hero.Inventory.Consumables);
        }

        public void LoadHeroForRepeatFight()
        {
            Hero.Stats.SetAll(_stats);
            _stats.Clear();

            foreach (Item item in _consumables)
            {
                if (!Hero.Inventory.Consumables.Contains(item))
                {
                    Hero.Inventory.AddItem(item);
                }
            }

            _consumables.Clear();
        }
    }
}