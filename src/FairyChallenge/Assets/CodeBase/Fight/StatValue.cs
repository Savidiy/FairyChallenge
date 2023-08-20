namespace Fairy
{
    internal class StatValue
    {
        public int Value;
        private readonly int _maxValue;
        private readonly int _minValue;

        public StatValue(int value, int maxValue = int.MaxValue, int minValue = int.MinValue)
        {
            Value = value;
            _maxValue = maxValue;
            _minValue = minValue;
        }

        public void Set(int value)
        {
            Value = value;
            if (Value > _maxValue)
                Value = _maxValue;
            if (Value < _minValue)
                Value = _minValue;
        }
    }
}