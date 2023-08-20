using System.Collections.Generic;
using UnityEngine.Pool;

namespace Fight
{
    internal class ActionResultLogger
    {
        private readonly Dictionary<Hero, Dictionary<StatType, int>> _savedStats = new();
        private readonly List<ActionResultChange> _changes = new();
        private readonly Dictionary<StatType, int> _currentStats = new();

        public void SaveState(List<Hero> heroes)
        {
            Clear();
            foreach (Hero hero in heroes)
            {
                var dictionary = DictionaryPool<StatType, int>.Get();
                hero.Stats.FillAllStatsValues(dictionary);
                _savedStats.Add(hero, dictionary);
            }
        }

        private void Clear()
        {
            foreach ((Hero _, Dictionary<StatType, int> value) in _savedStats)
                DictionaryPool<StatType, int>.Release(value);

            _savedStats.Clear();
            _changes.Clear();
        }

        public void ApplyState(List<Hero> heroes)
        {
            foreach (Hero hero in heroes)
            {
                if (!_savedStats.TryGetValue(hero, out Dictionary<StatType, int> savedStats))
                    continue;

                AddHeroChanges(hero, savedStats);
            }
        }

        private void AddHeroChanges(Hero hero, Dictionary<StatType, int> savedStats)
        {
            _currentStats.Clear();
            hero.Stats.FillAllStatsValues(_currentStats);

            foreach ((StatType statType, int newValue) in _currentStats)
            {
                if (savedStats.TryGetValue(statType, out int savedValue))
                {
                    if (savedValue != newValue)
                        _changes.Add(new ActionResultChange(hero, statType, savedValue, newValue));
                }
                else
                {
                    _changes.Add(new ActionResultChange(hero, statType, 0, newValue));
                }
            }
        }

        public string PrintLog()
        {
            return string.Join(", ", _changes);
        }
    }
}