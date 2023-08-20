using System;
using System.Collections.Generic;
using Savidiy.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fairy
{
    [CreateAssetMenu(fileName = nameof(EncounterLibrary), menuName = nameof(EncounterLibrary), order = 0)]
    public class EncounterLibrary : AutoSaveScriptableObject
    {
        [ListDrawerSettings(ListElementLabelName = "@this")]
        public List<EncounterStaticData> Encounters = new();

        public readonly ValueDropdownList<string> EncounterIds = new();

        public EncounterStaticData GetStaticData(string encounterId)
        {
            foreach (EncounterStaticData hero in Encounters)
                if (hero.EncounterId.Equals(encounterId))
                    return hero;

            Debug.LogError($"Encounter with id '{encounterId}' not found");
            return new EncounterStaticData() {EncounterId = encounterId};
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            EncounterIds.Clear();
            foreach (EncounterStaticData hero in Encounters)
                EncounterIds.Add(hero.EncounterId);
        }
    }

    [Serializable]
    public class EncounterStaticData
    {
        public string EncounterId = string.Empty;

        public override string ToString()
        {
            return EncounterId;
        }
    }
}