using System;
using System.Collections.Generic;
using Savidiy.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fight
{
    [CreateAssetMenu(fileName = nameof(AttackLibrary), menuName = nameof(AttackLibrary), order = 0)]
    public class AttackLibrary : AutoSaveScriptableObject
    {
        public List<AttackStaticData> Attacks = new();

        public readonly ValueDropdownList<string> AttackIds = new();

        public AttackStaticData GetStaticData(string attackId)
        {
            foreach (AttackStaticData attackStaticData in Attacks)
                if (attackStaticData.AttackId.Equals(attackId))
                    return attackStaticData;

            Debug.LogError($"Attack with id '{attackId}' not found");
            return new AttackStaticData() {AttackId = attackId};
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            AttackIds.Clear();
            foreach (AttackStaticData attack in Attacks)
                    AttackIds.Add(attack.AttackId);
        }
    }

    [Serializable]
    public class AttackStaticData
    {
        public string AttackId = string.Empty;
        public List<EffectStaticData> Effects = new();
    }
    
    [Serializable]
    public class EffectStaticData
    {
        public EffectType EffectType;
        public int Power;
    }

    public enum EffectType
    {
        Damage,
        Heal,
        HealByDamage,
        BuffSelfAttack,
        BuffSelfDefence,
        DebuffEnemyAttack,
        DebuffEnemyDefence,
    }
}