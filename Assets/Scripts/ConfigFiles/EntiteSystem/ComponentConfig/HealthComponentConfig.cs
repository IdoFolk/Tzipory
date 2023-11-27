using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.ComponentConfig
{
    [System.Serializable]
    public struct HealthComponentConfig 
    {
        [SerializeField] public HealthComponentType HealthComponentType;
        [SerializeField] public float HealthStat;
        [SerializeField,ShowIf(nameof(HealthComponentType),HealthComponentType.InvincibleTime)] public float InvincibleTimeStat;
        [SerializeField,ShowIf(nameof(HealthComponentType),HealthComponentType.Regen)] public float RegenSpeedStat;
    }

    public enum HealthComponentType
    {
        Regen,
        InvincibleTime,
        Standard,
    }
}