﻿using System.Collections.Generic;
using Savidiy.Utils;
using static Fight.FightAutoTests;

namespace Fight
{
    internal class TestResultSummary
    {
        public readonly Dictionary<StatType, int> Stats = new();
        public readonly bool HeroIsAlive;
        public readonly bool EnemyIsAlive;
        public readonly int LastTurn;
        private readonly string _printCurrentVariant;
        private Hero _hero;
        private Hero _enemy;
        public int Count { get; private set; } = 1;

        public TestResultSummary(Hero hero, Hero enemy, int lastTurn, string printCurrentVariant)
        {
            _enemy = enemy;
            _hero = hero;
            _printCurrentVariant = printCurrentVariant;
            LastTurn = lastTurn;
            HeroIsAlive = hero.IsAlive;
            hero.Stats.FillAllStatsValues(Stats);
            EnemyIsAlive = enemy.IsAlive;
        }

        public bool IsSame(Hero hero, Hero enemy, int lastTurn)
        {
            if (LastTurn != lastTurn)
                return false;

            if (HeroIsAlive != hero.IsAlive)
                return false;

            if (EnemyIsAlive != enemy.IsAlive)
                return false;
            
            if (!HeroIsAlive)
                return true;

            foreach ((StatType statType, int value) in Stats)
                if (value != hero.Stats.Get(statType))
                    return false;

            return true;
        }

        public void IncreaseCount()
        {
            Count++;
        }

        public string PrintLog(int sum)
        {
            var percent = (int) (Count * 100f / sum);
            return
                $"{_hero.ForConsole} {PrintAlive(HeroIsAlive)}, {_enemy.ForConsole} {PrintAlive(EnemyIsAlive)}, LastTurn: {LastTurn}, HP={Stats[StatType.HealthPoints].Color(ConsoleColor.RED)}/{Stats[StatType.MaxHealthPoints]}, Att={Stats[StatType.Attack]}, Def={Stats[StatType.Defence]}, {percent}% {Count}/{sum} ex. {_printCurrentVariant}";
        }
    }
}