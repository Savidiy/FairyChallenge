namespace Fight
{
    public sealed class ItemFactory
    {
        private readonly ItemsLibrary _itemsLibrary;

        public ItemFactory(ItemsLibrary itemsLibrary)
        {
            _itemsLibrary = itemsLibrary;
        }

        public Item Create(string itemId)
        {
            ItemStaticData itemStaticData = _itemsLibrary.GetItemData(itemId);
            Item item = new Item(itemStaticData);
            return item;
        }
    }
}