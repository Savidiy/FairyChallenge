using System.Collections.Generic;

namespace Fight
{
    public sealed class Inventory
    {
        public IReadOnlyList<Item> Consumables => _consumables;
        private readonly List<Item> _consumables = new();
        
        public void AddConsumable(Item item)
        {
            _consumables.Add(item);
        }

        public Item TakeConsumable(int itemIndex)
        {
            Item item = _consumables[itemIndex];
            _consumables.RemoveAt(itemIndex);
            return item;
        }
    }
}