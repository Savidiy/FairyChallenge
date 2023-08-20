using System;
using System.Collections.Generic;

namespace Fairy
{
    public sealed class Inventory
    {
        private readonly Dictionary<ItemType, List<Item>> _items = new();
        
        public IReadOnlyList<Item> Consumables => _items[ItemType.Consumable];

        public readonly ItemSlot UsedWeapon;
        public readonly ItemSlot UsedArmor;
        public readonly ItemSlot UsedAccessory;

        public Inventory(ItemEffectApplier itemEffectApplier)
        {
            UsedArmor = new ItemSlot(ItemType.Armor, itemEffectApplier);
            UsedAccessory = new ItemSlot(ItemType.Accessory, itemEffectApplier);
            UsedWeapon = new ItemSlot(ItemType.Weapon, itemEffectApplier);
            Array values = Enum.GetValues(typeof(ItemType));
            foreach (ItemType itemType in values)
                _items.Add(itemType, new List<Item>());
        }
        
        public void AddItem(Item item)
        {
            _items[item.ItemStaticData.ItemType].Add(item);
        }

        public Item TakeConsumable(int itemIndex)
        {
            List<Item> consumables = _items[ItemType.Consumable];
            Item item = consumables[itemIndex];
            consumables.RemoveAt(itemIndex);
            return item;
        }
    }
}