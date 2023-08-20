using Savidiy.Utils;
using Sirenix.OdinInspector;

namespace Fairy
{
    public static class OdinEncounterIdProvider
    {
        private static readonly EditorScriptableObjectLoader<EncounterLibrary> Loader = new();
        public static ValueDropdownList<string> EncounterIds => Loader.GetAsset().EncounterIds;
    }
}