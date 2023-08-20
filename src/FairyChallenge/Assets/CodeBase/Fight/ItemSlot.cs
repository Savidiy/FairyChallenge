using UnityEngine;

namespace Fight
{
    public sealed class ItemSlot
    {
        private readonly ItemEffectApplier _itemEffectApplier;
        private Item _item;

        public ItemType ItemType { get; }

        public ItemSlot(ItemType itemType, ItemEffectApplier itemEffectApplier)
        {
            _itemEffectApplier = itemEffectApplier;
            ItemType = itemType;
        }

        public bool TryGetItem(out Item item)
        {
            item = _item;
            return _item != null;
        }

        public void SetItem(Item item)
        {
            ItemType itemType = item.ItemStaticData.ItemType;
            if (ItemType != itemType)
            {
                Debug.LogError($"Item type mismatch. Expected {ItemType}, but got {itemType}");
                return;
            }
            
            if (TryGetItem(out Item currentItem))
                _itemEffectApplier.RemoveItemEffects(currentItem);

            _item = item;
            _itemEffectApplier.ApplyItemEffects(item);
        }
    }
}