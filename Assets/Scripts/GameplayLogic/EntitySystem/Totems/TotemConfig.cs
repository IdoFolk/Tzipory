using Tzipory.ConfigFiles.StatusSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.Totems
{
    [CreateAssetMenu(fileName = "NewTotemConfig", menuName = "ScriptableObjects/EntitySystem/Totems/New Totem Config", order = 0)]
    public class TotemConfig : ScriptableObject
    {
        [Range(0, 20)] public float Range;
        public Color RingColor;
        public StatEffectConfig StatEffectConfig;
        public Sprite TotemSprite;
        public TotemEffectUnitType TotemEffectUnitType;
        public float TotemEffectInterval;
        public float TotemCooldown;

    }

    public enum TotemEffectUnitType
    {
        Enemy,
        Shaman
    }
}