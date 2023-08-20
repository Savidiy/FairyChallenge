using System;
using Sirenix.OdinInspector;

namespace Fairy
{
    [Serializable]
    public class FightTestId
    {
        [ValueDropdown(nameof(TestIds)), HorizontalGroup] public string TestId;
        private ValueDropdownList<string> TestIds => OdinFightTestIdProvider.TestIds;
    }
}