using System;
using Tzipory.ConfigFiles.StatusSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    [CreateAssetMenu(fileName = "NewPowerStructureConfig", menuName = "ScriptableObjects/EntitySystem/PowerStructure/New Power Structure Config", order = 0)]
    public class PowerStructureConfig : ScriptableObject
    {
        [Range(0, 20)] public float Range;
        [Range(0, 1)] public float[] RingsRanges;
        public Color RingDefaultColor;
        public Color PowerStructureTypeColor;
        public float DefaultSpriteAlpha;
        public float SpriteAlphaFade;
        public StatEffectConfig StatEffectConfig;
        public Sprite PowerStructureSprite;

        private void OnValidate()
        {
            if (StatEffectConfig.StatModifier.RingModifiers.Length != RingsRanges.Length)
                StatEffectConfig.StatModifier.ChangeRingModifiersNumber(RingsRanges.Length);
        }
    }
}