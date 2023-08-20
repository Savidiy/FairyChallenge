using System.Collections.Generic;

namespace Fight
{
    public sealed class HeroStats
    {
        private readonly Dictionary<StatType, StatValue> _values = new();

        public bool IsAlive => Get(StatType.HealthPoints) > 0;

        public HeroStats(HeroStaticData heroStaticData)
        {
            _values.Add(StatType.Attack, new StatValue(heroStaticData.Attack, minValue: 1));
            _values.Add(StatType.Defence, new StatValue(heroStaticData.Defence, minValue: 1));
            int hp = heroStaticData.HealthPoints;
            _values.Add(StatType.MaxHealthPoints, new StatValue(hp));
            _values.Add(StatType.HealthPoints, new StatValue(hp, maxValue: hp));
        }

        public int Get(StatType statType)
        {
            return _values.TryGetValue(statType, out StatValue result) 
                ? result.Value 
                : 0;
        }

        private void Set(StatType statType, int value)
        {
            _values[statType].Set(value);
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
            return $"HP {Get(StatType.HealthPoints)}/{Get(StatType.MaxHealthPoints)} " +
                   $"Att {Get(StatType.Attack)}, " +
                   $"Def {Get(StatType.Defence)}";
        }

        public void FillAllStatsValues(Dictionary<StatType, int> dictionary)
        {
            foreach ((StatType key, StatValue value) in _values)
                dictionary.Add(key, value.Value);
        }
    }
}