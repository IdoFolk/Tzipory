using Sirenix.OdinInspector;
using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers.Consts;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.Totems
{
    [CreateAssetMenu(fileName = "NewTotemConfig", menuName = "ScriptableObjects/EntitySystem/Totems/New Totem Config", order = 0)]
    public class TotemConfig : ScriptableObject , IConfigFile
    {
        [Range(0, 20)] public float Range;
        public Color RingColor;
        public TotemEffectUnitType TotemEffectUnitType;
        public Sprite TotemSprite;
        public float TotemEffectInterval;
        public float HoldClickWaitTime;
        [TabGroup("Totem Stats")]public StatConfig[] TotemStatConfigs;
        [TabGroup("Ability Stat Effect")] public StatEffectConfig StatEffectConfig;

        public int ObjectId => name.GetHashCode();
        public int ConfigTypeId => Constant.DataId.TOTEM_DATA_ID;
    }

    public enum TotemEffectUnitType
    {
        Enemy,
        Shaman
    }
}