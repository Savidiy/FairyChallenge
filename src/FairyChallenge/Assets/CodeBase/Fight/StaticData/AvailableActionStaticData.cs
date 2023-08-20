using System;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace Fairy
{
    [Serializable]
    public class AvailableActionStaticData
    {
        [FormerlySerializedAs("AttackId")] [ValueDropdown(nameof(ActionIds)), HideLabel, HorizontalGroup] public string ActionId;
        private ValueDropdownList<string> ActionIds => OdinActionIdProvider.ActionIds;
    }
}