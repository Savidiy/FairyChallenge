using System;
using System.Collections.Generic;
using Savidiy.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fairy
{
    [CreateAssetMenu(fileName = nameof(NodesLibrary), menuName = nameof(NodesLibrary), order = 0)]
    public class NodesLibrary : AutoSaveScriptableObject
    {
        [ListDrawerSettings(ListElementLabelName = "@this")]
        public List<NodeStaticData> Nodes;

        public ValueDropdownList<string> NodeIds;

        protected override void OnValidate()
        {
            base.OnValidate();

            NodeIds = new ValueDropdownList<string>();
            foreach (NodeStaticData node in Nodes)
                NodeIds.Add(node.NodeId);
        }

        public NodeStaticData GetNodeStaticData(string currentNodeId)
        {
            foreach (NodeStaticData node in Nodes)
                if (node.NodeId.Equals(currentNodeId))
                    return node;

            Debug.LogError($"NodeStaticData '{currentNodeId}' not found");
            return new NodeStaticData() {NodeId = currentNodeId};
        }
    }

    [Serializable]
    public class NodeStaticData
    {
        public string NodeId = string.Empty;
        public List<StepStaticData> Steps = new();
        public override string ToString() => NodeId;
    }

    [Serializable]
    public class StepStaticData
    {
        [EnumToggleButtons, LabelWidth(40)] public StepType Type;
        private bool ShowGoto => Type == StepType.Goto;
        [ShowIf(nameof(ShowGoto)), ValueDropdown(nameof(NodeIds))] public string NodeId = string.Empty;
        private ValueDropdownList<string> NodeIds => OdinNodeIdProvider.NodeIds;

        private bool ShowBackground => Type == StepType.Background;
        [ShowIf(nameof(ShowBackground)), ValueDropdown(nameof(BackgroundIds))] public string BackgroundId = string.Empty;
        private ValueDropdownList<string> BackgroundIds => OdinBackgroundIdProvider.BackgroundIds;

        private bool ShowDialog => Type == StepType.Dialog;
        [ShowIf(nameof(ShowDialog)), ValueDropdown(nameof(PersonIds))] public string PersonId = string.Empty;
        private ValueDropdownList<string> PersonIds => OdinPersonIdProvider.PersonIds;
        [ShowIf(nameof(ShowDialog)), LabelWidth(40)] public string Text;

        private bool ShowButton => Type == StepType.Button;
        [ShowIf(nameof(ShowButton)), LabelWidth(40)] public string ButtonText;
        [ShowIf(nameof(ShowButton)), ValueDropdown(nameof(NodeIds))] public string ButtonNodeId = string.Empty;
    }
}