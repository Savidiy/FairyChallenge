using System;
using System.Collections.Generic;
using Savidiy.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fairy
{
    [CreateAssetMenu(fileName = nameof(BackgroundLibrary), menuName = nameof(BackgroundLibrary), order = 0)]
    public sealed class BackgroundLibrary : AutoSaveScriptableObject
    {
        public BackgroundStaticData DefaultBackground;

        [ListDrawerSettings(ListElementLabelName = "@this")]
        public List<BackgroundStaticData> Backgrounds;

        public readonly ValueDropdownList<string> BackgroundIds = new();

        protected override void OnValidate()
        {
            base.OnValidate();

            BackgroundIds.Clear();
            BackgroundIds.Add(DefaultBackground.BackgroundId);
            foreach (BackgroundStaticData node in Backgrounds)
                BackgroundIds.Add(node.BackgroundId);
        }

        public BackgroundStaticData GetBackground(string backgroundId)
        {
            if (backgroundId.Equals(DefaultBackground.BackgroundId))
                return DefaultBackground;

            foreach (BackgroundStaticData background in Backgrounds)
                if (background.BackgroundId.Equals(backgroundId))
                    return background;

            Debug.LogError($"BackgroundStaticData '{backgroundId}' not found");
            return DefaultBackground;
        }
    }

    [Serializable]
    public class BackgroundStaticData
    {
        public string BackgroundId = string.Empty;
        public Sprite Sprite;
        public Color Color = new Color(1, 1, 1, 1);
        public override string ToString() => BackgroundId;
    }
}