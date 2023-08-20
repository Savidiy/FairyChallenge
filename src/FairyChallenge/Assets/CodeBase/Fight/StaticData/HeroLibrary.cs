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
        }
    }

    [Serializable]
    public class HeroStaticData
    {
        private const int MAX_HEALTH_POINTS = 30;
        private const int MAX_ATTACK = 30;
        private const int MAX_DEFENCE = 30;

        public string HeroId = string.Empty;

        public List<AvailableAttackStaticData> Attacks;
        [ProgressBar(1, nameof(MAX_HEALTH_POINTS), r: 0, g: 1, b: 0)] public int HealthPoints;
        [ProgressBar(1, nameof(MAX_ATTACK), r: 1, g: 0, b: 0)] public int Attack;
        [ProgressBar(1, nameof(MAX_DEFENCE), r: 0, g: 0, b: 1)] public int Defence;

        public override string ToString()
        {
            return HeroId;
        }
    }
}