using Savidiy.Utils;
using Sirenix.OdinInspector;

namespace Fight
{
    public static class OdinAttackIdProvider
    {
        private static readonly EditorScriptableObjectLoader<AttackLibrary> Loader = new();
        public static readonly ValueDropdownList<string> AttackIds = Loader.GetAsset().AttackIds;
    }
}