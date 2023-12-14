using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.Visual;
using UnityEngine;

namespace Tzipory.ConfigFiles.AbilitySystem
{
    [System.Serializable]
    public struct AbilityVisualConfig
    {
        [SerializeField] public AbilityVisualType _abilityVisualType;
        [SerializeField,ShowIf(nameof(_abilityVisualType),AbilityVisualType.TimeLine)] public AnimationConfig AbilityAnimationConfig;
        [SerializeField] public GameObject VisualObject;
        
        [SerializeField,TabGroup("Ability Visual Config"),Tooltip("")] public bool HaveEffectOnEntity;
        [SerializeField, TabGroup("Ability Visual Config"), Tooltip(""), ShowIf(nameof(HaveEffectOnEntity))] public AnimationConfig TargetAnimationConfig;
    }

    public enum AbilityVisualType
    {
        VisualObject,
        TimeLine
    }
}