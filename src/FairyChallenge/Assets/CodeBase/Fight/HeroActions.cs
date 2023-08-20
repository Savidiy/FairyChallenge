using System.Collections.Generic;

namespace Fairy
{
    public sealed class HeroActions
    {
        public int Count => _actions.Count;
        public IReadOnlyList<ActionData> Actions => _actions;
        private readonly List<ActionData> _actions = new();

        public HeroActions(IReadOnlyList<ActionData> actions)
        {
            _actions.AddRange(actions);
        }

        public void AddAction(ActionData actionData)
        {
            _actions.Add(actionData);
        }

        public void RemoveAction(string actionId)
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                ActionData actionData = _actions[i];
                if (actionData.ActionId.Equals(actionId))
                {
                    _actions.RemoveAt(i);
                    return;
                }
            }
        }
    }
}