using System;
using System.Collections.Generic;

namespace Fight
{
    public sealed class AttackResult
    {
        private readonly Dictionary<Hero, List<StatChangeData>> _changes = new();

        public string AttackId { get; private set; }

        public void AddChange(Hero hero, StatChangeData statChangeData)
        {
            if (!_changes.TryGetValue(hero, out List<StatChangeData> heroChanges))
            {
                heroChanges = new List<StatChangeData>();
                _changes.Add(hero, heroChanges);
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
                result += $"{hero} {string.Join(", ", list)}";
            }

            return result;
        }

        public void SetAttackId(string attackId)
        {
            AttackId = attackId;
        }
    }
}