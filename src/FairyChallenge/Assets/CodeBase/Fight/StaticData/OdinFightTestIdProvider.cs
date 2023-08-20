using System;
using Savidiy.Utils;
using Sirenix.OdinInspector;

namespace Fight
{
    [Serializable]
    public static class OdinFightTestIdProvider
    {
        private static readonly EditorScriptableObjectLoader<FightTestLibrary> Loader = new();
        public static ValueDropdownList<string> TestIds => Loader.GetAsset().TestIds;
    }
}