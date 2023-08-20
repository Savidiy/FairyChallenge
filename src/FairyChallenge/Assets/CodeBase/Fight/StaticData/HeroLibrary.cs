using System;
using System.Collections.Generic;
using Savidiy.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fight
{
    [CreateAssetMenu(fileName = nameof(HeroLibrary), menuName = nameof(HeroLibrary), order = 0)]
    public class HeroLibrary : AutoSaveScriptableObject
    {
        [ListDrawerSettings(ListElementLabelName = "@this")]
        public List<HeroStaticData> Heroes = new();

        public readonly ValueDropdownList<string> HeroIds = new();

        public HeroStaticData GetStaticData(string heroId)
        {
            foreach (HeroStaticData hero in Heroes)
                if (hero.HeroId.Equals(heroId))
                    return hero;

            Debug.LogError($"Hero with id '{heroId}' not found");
            return new HeroStaticData() {HeroId = heroId};
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            HeroIds.Clear();
            foreach (HeroStaticData hero in Heroes)
                HeroIds.Add(hero.HeroId);

            foreach (HeroStaticData hero in Heroes)
            {
                if (hero.Stats.Count == 0)
                    hero.Stats.Add(new HeroStatStaticData());

                InitializeStats(hero);
            }
        }

        private static void InitializeStats(HeroStaticData hero)
        {
            for (var index = 0; index < hero.Stats.Count; index++)
            {
                if (index == 0)
                    continue;

                HeroStatStaticData heroStat = hero.Stats[index];
                if (heroStat.IsInitialized)
                    continue;

                HeroStatStaticData previousStat = hero.Stats[index - 1];

                heroStat.IsInitialized = true;
                heroStat.HealthPoints = previousStat.HealthPoints + hero.DefaultHealthPointsDelta;
                heroStat.Attack = previousStat.Attack + hero.DefaultAttackDelta;
                heroStat.Defence = previousStat.Defence + hero.DefaultDefenceDelta;
            }
        }
    }

    [Serializable]
    public class HeroStaticData
    {
        private const string DEFAULT_LEVEL_UP = "Default level up values";
        public string HeroId = string.Empty;

        public List<AvailableAttackStaticData> Attacks;

        [FoldoutGroup(DEFAULT_LEVEL_UP)] public int DefaultHealthPointsDelta;
        [FoldoutGroup(DEFAULT_LEVEL_UP)] public int DefaultAttackDelta;
        [FoldoutGroup(DEFAULT_LEVEL_UP)] public int DefaultDefenceDelta;

        [TableList(ShowIndexLabels = true)]
        public List<HeroStatStaticData> Stats;

        private HeroStatStaticData OnAddStat()
        {
            var stat = new HeroStatStaticData();
            if (Stats.Count > 0)
            {
                HeroStatStaticData lastStat = Stats[^1];
                stat.HealthPoints = lastStat.HealthPoints + DefaultHealthPointsDelta;
                stat.Attack = lastStat.Attack + DefaultAttackDelta;
                stat.Defence = lastStat.Defence + DefaultDefenceDelta;
            }

            return stat;
        }

        public override string ToString()
        {
            return HeroId;
        }
    }

    [Serializable]
    public sealed class HeroStatStaticData
    {
        [HideInInspector] public bool IsInitialized;
        public int HealthPoints;
        public int Attack;
        public int Defence;
    }
}