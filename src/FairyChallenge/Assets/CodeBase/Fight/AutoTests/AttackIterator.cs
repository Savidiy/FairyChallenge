using System.Collections.Generic;
using UnityEngine;

namespace Fight
{
    internal class AttackIterator
    {
        private readonly int _secondAttackCount;
        private readonly int _oneTurnVariantsCount;
        private readonly List<int> _currentVariant = new();
        private int _longestFightTurn = 0;

        public AttackIterator(Hero hero, Hero enemy)
        {
            int firstAttackCount = hero.AvailableAttacks.Count;
            _secondAttackCount = enemy.AvailableAttacks.Count;
            _oneTurnVariantsCount = firstAttackCount * _secondAttackCount;
            _currentVariant.Clear();
        }

        public int CurrentVariantNumber()
        {
            var index = 0;
            var multiplier = 1;
            int count = _currentVariant.Count;
            for (int i = _longestFightTurn - 1; i >= 0; i--)
            {
                if (i < count)
                    index += _currentVariant[i] * multiplier;
                multiplier *= _oneTurnVariantsCount;
            }

            return index;
        }

        public int EstimateVariantsCount()
        {
            return Mathf.RoundToInt(Mathf.Pow(_oneTurnVariantsCount, _longestFightTurn));
        }

        public bool HasNext()
        {
            foreach (int i in _currentVariant)
            {
                if (i != 0)
                    return true;
            }

            return false;
        }

        public void GetIndexForTurn(int turn, out int firstIndex, out int secondIndex)
        {
            _longestFightTurn = Mathf.Max(_longestFightTurn, turn + 1);
            for (int i = _currentVariant.Count; i <= turn; i++)
                _currentVariant.Add(0);

            int variant = _currentVariant[turn];
            GetAttackIndexes(variant, out firstIndex, out secondIndex);
        }

        public string PrintCurrentVariant()
        {
            string result = "";
            foreach (int i in _currentVariant)
            {
                GetAttackIndexes(i, out int firstIndex, out int secondIndex);
                result += $"{firstIndex}{secondIndex},";
            }
            return result;
        }

        private void GetAttackIndexes(int variant, out int firstIndex, out int secondIndex)
        {
            if (_secondAttackCount > 1)
            {
                firstIndex = variant / _secondAttackCount;
                secondIndex = variant % _secondAttackCount;
            }
            else
            {
                firstIndex = variant;
                secondIndex = 0;
            }
        }

        public void FightEndOnTurn(int turn)
        {
            for (int i = _currentVariant.Count - 1; i > turn; i--)
                _currentVariant.RemoveAt(i);

            for (int i = _currentVariant.Count - 1; i >= 0; i--)
            {
                _currentVariant[i]++;
                if (_currentVariant[i] >= _oneTurnVariantsCount)
                    _currentVariant[i] = 0;
                else
                    break;
            }
        }
    }
}