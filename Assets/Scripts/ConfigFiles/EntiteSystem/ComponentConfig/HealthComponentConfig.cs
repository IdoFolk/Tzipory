using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.ComponentConfig
{
    [System.Serializable]
    public struct HealthComponentConfig 
    {
        [SerializeField] public HealthComponentType HealthComponentType;
        [SerializeField] public float HealthStat;
        [SerializeField] public float InvincibleTimeStat;
    }

    public enum HealthComponentType
    {
        Regen,
        InvincibleTime,
        Standard,
    }
}