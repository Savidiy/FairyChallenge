using System;
using Sirenix.OdinInspector;

namespace Fight
{
    [Serializable]
    public class AvailableAttackStaticData
    {
        [HorizontalGroup, LabelText("From Lvl")] public int AvailableFromLevel;
        [ValueDropdown(nameof(AttackIds)), HideLabel, HorizontalGroup] public string AttackId;
        private ValueDropdownList<string> AttackIds => OdinAttackIdProvider.AttackIds;
    }
}