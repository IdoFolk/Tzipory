using Tzipory.ConfigFiles.StatusSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    [CreateAssetMenu(fileName = "NewPowerStructureConfig", menuName = "ScriptableObjects/EntitySystem/PowerStructure/New Power Structure Config", order = 0)]
    public class PowerStructureConfig : ScriptableObject
    {
        [Range(0, 20)] public float Range;
        [Range(0, 1)] public float[] RingsRatios;
        public Color RingOnHoverColor;
        public Color RingOnShamanHoverColor;
        public float DefaultSpriteAlpha;
        public float SpriteAlphaFade;
        public StatEffectConfig StatEffectConfig;
        public Sprite PowerStructureSprite;
    }
}