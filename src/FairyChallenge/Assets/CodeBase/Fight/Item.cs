namespace Fairy
{
    public class Item
    {
        public ItemStaticData ItemStaticData { get; }

        public Item(ItemStaticData itemStaticData)
        {
            ItemStaticData = itemStaticData;
        }
    }
}