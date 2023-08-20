using Savidiy.Utils;
using static Savidiy.Utils.ConsoleColor;

namespace Fight
{
    internal class ActionResultChange
    {
        private readonly string _log;

        public ActionResultChange(Hero hero, StatType statType, int savedValue, int newValue)
        {
            int delta = newValue - savedValue;
            _log = $"{hero.ForConsole} {statType.ToStringCashed().Color(WHITE)} {savedValue} {(delta >= 0 ? $"+ {delta}".Color(GREEN) : $"- {-delta}".Color(RED))} = {newValue.Color(WHITE)}";
        }

        public override string ToString()
        {
            return _log;
        }
    }
}