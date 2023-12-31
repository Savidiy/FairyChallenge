using System;
using System.Collections.Generic;

namespace Fairy
{
    public sealed class ActionResult
    {
        private readonly Dictionary<Hero, List<StatChangeData>> _changes = new();

        public string ActionId { get; private set; }

        public void AddChange(StatChangeData statChangeData)
        {
            Hero target = statChangeData.Target;
            if (!_changes.TryGetValue(target, out List<StatChangeData> heroChanges))
            {
                heroChanges = new List<StatChangeData>();
                _changes.Add(target, heroChanges);
            }

            heroChanges.Add(statChangeData);
        }

        public IReadOnlyList<StatChangeData> GetChanges(Hero hero)
        {
            if (_changes.TryGetValue(hero, out List<StatChangeData> heroChange))
                return heroChange;

            return Array.Empty<StatChangeData>();
        }

        public override string ToString()
        {
            string result = string.Empty;
            foreach ((Hero hero, List<StatChangeData> list) in _changes)
            {
                result += $"{hero.ForConsole} {string.Join(", ", list)}";
            }

            return result;
        }

        public void SetActionId(string actionId)
        {
            ActionId = actionId;
        }
    }
}