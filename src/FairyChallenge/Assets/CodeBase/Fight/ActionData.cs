using System.Collections.Generic;

namespace Fight
{
    public sealed class ActionData
    {
        private readonly ActionStaticData _actionStaticData;
        public IReadOnlyList<EffectStaticData> Effects => _actionStaticData.Effects;
        public string ActionId => _actionStaticData.ActionId;

        public ActionData(ActionStaticData actionStaticData)
        {
            _actionStaticData = actionStaticData;
        }
    }
}