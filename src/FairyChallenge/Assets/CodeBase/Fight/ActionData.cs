using System.Collections.Generic;

namespace Fairy
{
    public sealed class ActionData
    {
        private readonly ActionStaticData _actionStaticData;
        public IReadOnlyList<EffectStaticData> Effects => _actionStaticData.Effects;
        public string ActionId => _actionStaticData.ActionId;
        public string UseText => _actionStaticData.UseText;

        public ActionData(ActionStaticData actionStaticData)
        {
            _actionStaticData = actionStaticData;
        }
    }
}