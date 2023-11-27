using Tzipory.ConfigFiles.AbilitySystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.ComponentConfig
{
    [System.Serializable]
    public struct AbilityComponentConfig 
    {
        [SerializeField] public AbilityConfig[] _abilityConfigs;
    }
}