namespace Fight
{
    internal class StatValue
    {
        public int Value;
        private int _maxValue;
        private int _minValue;

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
                _maxValue = Value;
            if (Value < _minValue)
                _minValue = Value;
        }
    }
}