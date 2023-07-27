using System.Collections.Generic;
using Helpers.Consts;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.EntitySystem.TargetingSystem;
using UnityEngine;

namespace Tzipory.AbilitiesSystem.AbilityConfigSystem
{
    [CreateAssetMenu(fileName = "NewAbilityConfig", menuName = "ScriptableObjects/EntitySystem/AbilitySystem/New ability config", order = 0)]
    public class AbilityConfig : ScriptableObject , IConfigFile
    {
        [SerializeField,Tooltip("")] private int _abilityId;
        [SerializeField,Tooltip("")] private string _abilityName;
        [Header("Ability config")]
        [SerializeField,Tooltip("")] private AbilityCastType abilityCastType;
        [SerializeField,ShowIf("abilityCastType",AbilityCastType.Projectile)] private float _projectileSpeed;
        [SerializeField,ShowIf("abilityCastType",AbilityCastType.Projectile)] private float _projectilePenetration;
        
        [SerializeField,Tooltip("")] private AbilityExecuteType abilityExecuteType;
        [SerializeField,ShowIf("abilityExecuteType",AbilityExecuteType.AOE)] private float _aoeRadius;
        [SerializeField,ShowIf("abilityExecuteType",AbilityExecuteType.AOE)] private float _aoeDuration;
        [SerializeField,ShowIf("abilityExecuteType",AbilityExecuteType.Chain)] private float _chainRadius;
        [SerializeField,ShowIf("abilityExecuteType",AbilityExecuteType.Chain)] private float _chainDuration;
        [SerializeField,ShowIf("abilityExecuteType",AbilityExecuteType.Chain)] private float _chainAmount;
        
        [Header("Targeting")]
        [SerializeField,Tooltip("")] private TargetingPriorityType targetingPriorityType;
        [Header("Ability parameters")]
        [SerializeField,Tooltip("")] private float _cooldown;
        [SerializeField,Tooltip("")] private float _castTime;
        [SerializeField,Tooltip("")] private List<StatusEffectConfig> _statusEffectConfigs;
        
        public string AbilityName => _abilityName;
        public int AbilityId => _abilityId;
        public AbilityExecuteType AbilityExecuteType => abilityExecuteType;
        public AbilityCastType AbilityCastType => abilityCastType;

        public TargetingPriorityType TargetingPriorityType => targetingPriorityType;
        
        public float ProjectileSpeed => _projectileSpeed;

        public float ProjectilePenetration => _projectilePenetration;
        
        public float AoeRadius => _aoeRadius;

        public float AoeDuration => _aoeDuration;

        public float ChainRadius => _chainRadius;

        public float ChainDuration => _chainDuration;

        public float ChainAmount => _chainAmount;

        public TargetingPriorityType TargetingPriorityType1 => targetingPriorityType;

        public float Cooldown => _cooldown;
        public float CastTime => _castTime;
        public List<StatusEffectConfig> StatusEffectConfigs => _statusEffectConfigs;

        public int ConfigTypeId => Constant.DataId.ABILITY_DATA_ID;
    }

    public enum AbilityExecuteType
    {
        AOE,
        Single,
        Chain
    }
    
    public enum AbilityCastType
    {
        Projectile,
        Instant,
        Self
    }

    public enum EffectType
    {
        Positive,
        Negative,
    }
}