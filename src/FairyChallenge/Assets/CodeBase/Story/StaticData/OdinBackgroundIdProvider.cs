using Savidiy.Utils;
using Sirenix.OdinInspector;

namespace Fairy
{
    public static class OdinBackgroundIdProvider
    {
        private static readonly EditorScriptableObjectLoader<BackgroundLibrary> Loader = new();
        public static ValueDropdownList<string> BackgroundIds => Loader.GetAsset().BackgroundIds;
    }
}