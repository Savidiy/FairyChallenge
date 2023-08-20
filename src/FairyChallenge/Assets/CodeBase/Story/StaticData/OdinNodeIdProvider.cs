using Savidiy.Utils;
using Sirenix.OdinInspector;

namespace Fairy
{
    public static class OdinNodeIdProvider
    {
        private static readonly EditorScriptableObjectLoader<NodesLibrary> Loader = new();
        public static ValueDropdownList<string> NodeIds => Loader.GetAsset().NodeIds;
    }
}