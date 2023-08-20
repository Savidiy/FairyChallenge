using System;
using Sirenix.OdinInspector;

namespace Fight
{
    [Serializable]
    public class AvailableAttackStaticData
    {
        [ValueDropdown(nameof(AttackIds)), HideLabel, HorizontalGroup] public string AttackId;
        private ValueDropdownList<string> AttackIds => OdinAttackIdProvider.AttackIds;
    }
}