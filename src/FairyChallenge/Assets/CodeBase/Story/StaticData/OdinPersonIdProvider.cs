using Savidiy.Utils;
using Sirenix.OdinInspector;

namespace Fairy
{
    public static class OdinPersonIdProvider
    {
        private static readonly EditorScriptableObjectLoader<PersonLibrary> Loader = new();
        public static ValueDropdownList<string> PersonIds => Loader.GetAsset().PersonIds;
    }
}