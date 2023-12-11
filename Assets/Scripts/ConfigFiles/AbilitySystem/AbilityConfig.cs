using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.TargetingSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.AbilitySystem
{
    [CreateAssetMenu(fileName = "NewAbilityConfig", menuName = "ScriptableObjects/EntitySystem/AbilitySystem/New ability config", order = 0)]
    public class AbilityConfig : ScriptableObject , IConfigFile
    {
        public readonly string  AbilityPrefabPath = "Configs/AbilitySystem/AbilityConfig";
        [SerializeField,Tooltip("")] public int AbilityId;
        [SerializeField,Tooltip("")] public string AbilityName;
        
        [Header("TargetingHandler")]
        [SerializeField,Tooltip("")] private TargetingPriorityType _targetingPriorityType;
        [Space]
        [SerializeField,TabGroup("Ability execute type config"),Tooltip("")] public ExecuterConfig AbilityExecute;
        
        [TabGroup("Ability execute type config")]public bool HaveSecondaryAbilityExecuteType;
        [SerializeField,TabGroup("Ability execute type config"),Tooltip(""),ShowIf(nameof(HaveSecondaryAbilityExecuteType))] public ExecuterConfig SecondaryAbilityExecute;
        
        [SerializeField,Tooltip(""),HideIf(nameof(HaveSecondaryAbilityExecuteType)),TabGroup("Stat effect")] public List<StatEffectConfig> StatusEffectConfigs;
        [SerializeField,Tooltip(""),HideIf(nameof(HaveSecondaryAbilityExecuteType)),TabGroup("Stat effect")] public bool DoExitEffects = false;
        [SerializeField,Tooltip(""),ShowIf(nameof(DoExitEffects)),TabGroup("Stat effect")] public List<StatEffectConfig> StatusEffectConfigsOnExit;
        
        [SerializeField,TabGroup("Ability parameters"),Tooltip("")] public float Cooldown;
        [SerializeField,TabGroup("Ability parameters"),Tooltip("")] public float CastTime;
        
        [SerializeField,TabGroup("Ability Visual Config"),Tooltip("")] public AbilityVisualConfig AbilityVisualConfig;
        public TargetingPriorityType TargetingPriorityType => _targetingPriorityType;
        public int ObjectId => AbilityId;
        public int ConfigTypeId => Constant.DataId.ABILITY_DATA_ID;
    }
    
    [System.Serializable]
    public class ExecuterConfig
    {
        [SerializeField,Tooltip("")] private AbilityExecuteType _abilityExecuteType;
        [SerializeField,ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.Projectile)] private float _projectileSpeed;
        [SerializeField,ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.Projectile)] private float _projectilePenetration;
        [SerializeField,ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.AOE)] private float _aoeRadius;
        [SerializeField,ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.AOE)] private float _aoeDuration;
        [SerializeField,ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.Chain)] private float _chainRadius;
        [SerializeField,ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.Chain)] private float _chainDuration;
        [SerializeField,ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.Chain)] private float _chainAmount;
        
        public AbilityExecuteType AbilityExecuteType => _abilityExecuteType;
        public float ProjectileSpeed => _projectileSpeed;
        public float ProjectilePenetration => _projectilePenetration;
        public float AoeRadius => _aoeRadius;
        public float AoeDuration => _aoeDuration;
        public float ChainRadius => _chainRadius;
        public float ChainDuration => _chainDuration;
        public float ChainAmount => _chainAmount;
    }

   

    public enum AbilityExecuteType
    {
        AOE,
        StatExecuter,
        Chain,
        Projectile
    }
}