using Savidiy.Utils;
using Sirenix.OdinInspector;

namespace Fairy
{
    public static class OdinItemIdProvider
    {
        private static readonly EditorScriptableObjectLoader<ItemsLibrary> Loader = new();
        public static ValueDropdownList<string> ItemIds => Loader.GetAsset().ItemIds;
        public static ValueDropdownList<string> GetItemIds(ItemType itemType) => Loader.GetAsset().GetItemIds(itemType);
    }
}