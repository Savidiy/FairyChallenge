using Savidiy.Utils;
using Sirenix.OdinInspector;

namespace Fairy
{
    public static class OdinHeroIdProvider
    {
        private static readonly EditorScriptableObjectLoader<HeroLibrary> Loader = new();
        public static ValueDropdownList<string> HeroIds => Loader.GetAsset().HeroIds;
    }
}