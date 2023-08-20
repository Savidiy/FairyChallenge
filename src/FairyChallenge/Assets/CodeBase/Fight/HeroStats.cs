using System.Collections.Generic;
using UnityEngine;

namespace Fight
{
    public sealed class HeroStats
    {
        private readonly Dictionary<StatType, int> _values = new();

        private readonly Dictionary<StatType, int> _minimalValues = new()
        {
            {StatType.Attack, 1}, {StatType.Defence, 1},
        };

        public bool IsAlive => Get(StatType.CurrentHealthPoints) > 0;

        public int Get(StatType statType)
        {
            if (_values.TryGetValue(statType, out var result))
                return result;

            return 0;
        }

        private void Set(StatType statType, int value)
        {
            _values.TryAdd(statType, 0);
            _values[statType] = value;
        }

        public void Init(List<HeroStatStaticData> heroStatStaticDatas, int level)
        {
            if (CheckLevelAvailable(heroStatStaticDatas, level))
                return;

            HeroStatStaticData heroStatStaticData = GetStats(heroStatStaticDatas, level);

            Set(StatType.Attack, heroStatStaticData.Attack);
            Set(StatType.Defence, heroStatStaticData.Defence);
            Set(StatType.MaxHealthPoints, heroStatStaticData.HealthPoints);
            Set(StatType.CurrentHealthPoints, heroStatStaticData.HealthPoints);
        }

        private static bool CheckLevelAvailable(List<HeroStatStaticData> heroStatStaticDatas, int level)
        {
            if (level < 0)
            {
                Debug.LogError("Level must be greater than zero");
                return true;
            }

            if (level > heroStatStaticDatas.Count)
            {
                Debug.LogError($"Level must be less than '{heroStatStaticDatas.Count}'");
                return true;
            }

            return false;
        }

        private HeroStatStaticData GetStats(List<HeroStatStaticData> heroStatStaticDatas, int level)
        {
            if (level < 0)
            {
                Debug.LogError("Level must be greater than zero");
                return new HeroStatStaticData();
            }

            if (level >= heroStatStaticDatas.Count)
            {
                Debug.LogError($"Level must be less than '{heroStatStaticDatas.Count}'");
                return new HeroStatStaticData();
            }

            HeroStatStaticData stat = heroStatStaticDatas[level];
            return stat;
        }

        public void ApplyChange(StatChangeData statChangeData)
        {
            StatType statType = statChangeData.StatType;
            int currentValue = Get(statType);
            int newValue = currentValue + statChangeData.Delta;
            if (_minimalValues.TryGetValue(statType, out int minimal))
                newValue = Mathf.Max(newValue, minimal);

            Set(statType, newValue);
        }

        public override string ToString()
        {
            return $"HP {Get(StatType.CurrentHealthPoints)}/{Get(StatType.MaxHealthPoints)} " +
                   $"Att {Get(StatType.Attack)}, " +
                   $"Def {Get(StatType.Defence)}";
        }

        public void FillAllStatsValues(Dictionary<StatType, int> dictionary)
        {
            foreach ((StatType key, int value) in _values)
                dictionary.Add(key, value);
        }
    }
}