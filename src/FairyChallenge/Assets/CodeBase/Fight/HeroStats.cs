using System.Collections.Generic;
using UnityEngine;

namespace Fight
{
    public sealed class HeroStats
    {
        private readonly Dictionary<StatType, int> _values = new();
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
            int experience = 0;
            for (var i = 0; i < level; i++)
            {
                HeroStatStaticData statStaticData = heroStatStaticDatas[i];
                experience += statStaticData.ExperienceForNextLevel;
            }

            Set(StatType.Attack, heroStatStaticData.Attack);
            Set(StatType.Defence, heroStatStaticData.Defence);
            Set(StatType.Speed, heroStatStaticData.Speed);
            Set(StatType.MaxHealthPoints, heroStatStaticData.HealthPoints);
            Set(StatType.CurrentHealthPoints, heroStatStaticData.HealthPoints);
            Set(StatType.CurrentExperience, experience);
            Set(StatType.ExperienceForNextLevel, experience + heroStatStaticData.ExperienceForNextLevel);
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
            Set(statType, newValue);
        }

        public override string ToString()
        {
            return $"HP {Get(StatType.CurrentHealthPoints)}/{Get(StatType.MaxHealthPoints)} " +
                   $"Att {Get(StatType.Attack)}, " +
                   $"Def {Get(StatType.Defence)}, " +
                   $"Spd {Get(StatType.Speed)}, " +
                   $"Exp {Get(StatType.CurrentExperience)}/{Get(StatType.ExperienceForNextLevel)}";
        }

        public void FillAllStatsValues(Dictionary<StatType,int> dictionary)
        {
            foreach ((StatType key, int value) in _values)
                dictionary.Add(key, value);
        }
    }
}