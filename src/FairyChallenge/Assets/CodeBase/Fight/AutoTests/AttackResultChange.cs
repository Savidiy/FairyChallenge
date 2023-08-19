namespace Fight
{
    internal class AttackResultChange
    {
        private readonly string _log;

        public AttackResultChange(Hero hero, StatType statType, int savedValue, int newValue)
        {
            int delta = newValue - savedValue;
            _log = $"{hero} {statType} {savedValue} {(delta >= 0 ? $"+ {delta}" : $"- {-delta}")} = {newValue}";
        }

        public override string ToString()
        {
            return _log;
        }
    }
}