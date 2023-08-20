using System.Collections.Generic;

namespace Fight
{
    public class ActionFactory
    {
        private readonly ActionLibrary _actionLibrary;

        public ActionFactory(ActionLibrary actionLibrary)
        {
            _actionLibrary = actionLibrary;
        }
        
        public List<ActionData> Create(List<AvailableActionStaticData> actions)
        {
            var result = new List<ActionData>();
            foreach (var actionStaticData in actions)
            {
                ActionData actionInstance = Create(actionStaticData.ActionId);
                result.Add(actionInstance);
            }

            return result;
        }

        public ActionData Create(string actionId)
        {
            var actionStaticData = _actionLibrary.GetStaticData(actionId);
            var actionData = new ActionData(actionStaticData);
            return actionData;
        }
    }
}