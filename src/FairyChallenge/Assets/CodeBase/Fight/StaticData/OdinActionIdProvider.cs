using Savidiy.Utils;
using Sirenix.OdinInspector;

namespace Fairy
{
    public static class OdinActionIdProvider
    {
        private static readonly EditorScriptableObjectLoader<ActionLibrary> Loader = new();
        public static ValueDropdownList<string> ActionIds => Loader.GetAsset().ActionIds;
    }
}