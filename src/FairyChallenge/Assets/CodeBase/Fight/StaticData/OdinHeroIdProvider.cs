using Savidiy.Utils;
using Sirenix.OdinInspector;

namespace Fight
{
    public static class OdinHeroIdProvider
    {
        private static readonly EditorScriptableObjectLoader<HeroLibrary> Loader = new();
        public static readonly ValueDropdownList<string> HeroIds = Loader.GetAsset().HeroIds;
    }
}