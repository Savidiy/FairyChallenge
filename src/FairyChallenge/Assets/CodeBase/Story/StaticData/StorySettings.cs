using Savidiy.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fairy
{
    [CreateAssetMenu(fileName = nameof(StorySettings), menuName = nameof(StorySettings), order = 0)]
    public class StorySettings : AutoSaveScriptableObject
    {
        [ValueDropdown(nameof(NodeIds))] public string FirstNodeId;
        private ValueDropdownList<string> NodeIds => OdinNodeIdProvider.NodeIds;
    }
}