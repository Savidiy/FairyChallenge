using System;
using System.Collections.Generic;
using Savidiy.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fight
{
    [CreateAssetMenu(fileName = nameof(ActionLibrary), menuName = nameof(ActionLibrary), order = 0)]
    public class ActionLibrary : AutoSaveScriptableObject
    {
        [FormerlySerializedAs("Attacks")] public List<ActionStaticData> Actions = new();

        public readonly ValueDropdownList<string> ActionIds = new();

        public ActionStaticData GetStaticData(string actionId)
        {
            foreach (ActionStaticData actionStaticData in Actions)
                if (actionStaticData.ActionId.Equals(actionId))
                    return actionStaticData;

            Debug.LogError($"Action with id '{actionId}' not found");
            return new ActionStaticData() {ActionId = actionId};
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            ActionIds.Clear();
            foreach (ActionStaticData actionStaticData in Actions)
                    ActionIds.Add(actionStaticData.ActionId);
        }
    }

    [Serializable]
    public class ActionStaticData
    {
        [FormerlySerializedAs("AttackId")] public string ActionId = string.Empty;
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