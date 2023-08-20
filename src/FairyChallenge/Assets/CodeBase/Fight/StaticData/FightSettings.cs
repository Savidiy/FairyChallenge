using Savidiy.Utils;
using UnityEngine;

namespace Fairy
{
    [CreateAssetMenu(fileName = nameof(FightSettings), menuName = nameof(FightSettings), order = 0)]
    public class FightSettings : AutoSaveScriptableObject
    {
        public float DefenceMultiplier = 2f;
        public int MaxTurns = 12;
    }
}