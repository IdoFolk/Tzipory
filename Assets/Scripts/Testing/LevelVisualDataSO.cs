using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tzipory.Testing
{
    [CreateAssetMenu()]
    public class LevelVisualDataSO : ScriptableObject
    {
        public Level_VisualData level_VisualData;

        public float GetRelativeFontSizeForDamage(float damage) => Mathf.Clamp(
            (level_VisualData.FontSizeRange.y - level_VisualData.FontSizeRange.x) *
            (damage - level_VisualData.DamageRange.x) /
            (level_VisualData.DamageRange.y - level_VisualData.DamageRange.x) + level_VisualData.FontSizeRange.x,
            level_VisualData.FontSizeRange.x, level_VisualData.FontSizeRange.y);
    }
}