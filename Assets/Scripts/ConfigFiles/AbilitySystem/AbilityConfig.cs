using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.TargetingSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace Tzipory.ConfigFiles.AbilitySystem
{
    [CreateAssetMenu(fileName = "NewAbilityConfig", menuName = "ScriptableObjects/EntitySystem/AbilitySystem/New ability config", order = 0)]
    public class AbilityConfig : ScriptableObject , IConfigFile
    {
        [SerializeField,Tooltip("")] private int _abilityId;
        [SerializeField,Tooltip("")] private string _abilityName;
        
        [Header("TargetingHandler")]
        [SerializeField,Tooltip("")] private TargetingPriorityType _targetingPriorityType;
        [Space]
        [SerializeField,TabGroup("Ability cast type config"),Tooltip("")] private AbilityCastType _abilityCastType;
        [SerializeField,TabGroup("Ability cast type config"),ShowIf(nameof(_abilityCastType),AbilityCastType.Projectile)] private float _projectileSpeed;
        [SerializeField,TabGroup("Ability cast type config"),ShowIf(nameof(_abilityCastType),AbilityCastType.Projectile)] private float _projectilePenetration;
        
        [SerializeField,TabGroup("Ability execute type config"),Tooltip("")] private AbilityExecuteType _abilityExecuteType;
        [SerializeField,TabGroup("Ability execute type config"),ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.AOE)] private float _aoeRadius;
        [SerializeField,TabGroup("Ability execute type config"),ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.AOE)] private float _aoeDuration;
        [SerializeField,TabGroup("Ability execute type config"),ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.Chain)] private float _chainRadius;
        [SerializeField,TabGroup("Ability execute type config"),ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.Chain)] private float _chainDuration;
        [SerializeField,TabGroup("Ability execute type config"),ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.Chain)] private float _chainAmount;
        
        [SerializeField,TabGroup("Ability parameters"),Tooltip("")] private float _cooldown;
        [SerializeField,TabGroup("Ability parameters"),Tooltip("")] private float _castTime;
        [SerializeField,TabGroup("Ability parameters"),Tooltip("")] private List<StatEffectConfig> _statusEffectConfigs;
        [SerializeField,TabGroup("Ability parameters"),Tooltip(""), ShowIf(nameof(_abilityExecuteType), AbilityExecuteType.AOE)] private bool _doExitEffects;
        [SerializeField,TabGroup("Ability parameters"),Tooltip(""), ShowIf(nameof(_doExitEffects))] private List<StatEffectConfig> _statusEffectConfigsOnExit;
        
        //abilityVisualConfig
        [SerializeField,TabGroup("Ability Visual Config"),Tooltip(""),ShowIf(nameof(_abilityCastType),AbilityCastType.Projectile)] private PlayableAsset _abilityProjectileSprite;
        [SerializeField,TabGroup("Ability Visual Config"),Tooltip(""),ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.AOE)] private PlayableAsset _abilityAoeSprite;
        [SerializeField,TabGroup("Ability Visual Config"),Tooltip(""),ShowIf(nameof(_abilityExecuteType),AbilityExecuteType.Chain)] private Sprite _abilityChainSprite;
        [SerializeField,TabGroup("Ability Visual Config"),Tooltip("")]
        private bool _haveEffectOnEntity;

        [SerializeField, TabGroup("Ability Visual Config"), Tooltip(""), ShowIf(nameof(_haveEffectOnEntity))]
        private EffectOnEntityConfig _effectOnEntityConfig;
        
        public string AbilityName => _abilityName;
        public int AbilityId => _abilityId;
        public AbilityExecuteType AbilityExecuteType => _abilityExecuteType;
        public AbilityCastType AbilityCastType => _abilityCastType;

        public TargetingPriorityType TargetingPriorityType => _targetingPriorityType;

        public List<StatEffectConfig> StatusEffectConfigsOnExit => _statusEffectConfigsOnExit;

        public PlayableAsset AbilityProjectileSprite => _abilityProjectileSprite;

        public PlayableAsset AbilityAoeSprite => _abilityAoeSprite;

        public Sprite AbilityChainSprite => _abilityChainSprite;

        public bool HaveEffectOnEntity => _haveEffectOnEntity;

        public EffectOnEntityConfig EffectOnEntityConfig => _effectOnEntityConfig;

        public float ProjectileSpeed => _projectileSpeed;

        public float ProjectilePenetration => _projectilePenetration;
        
        public float AoeRadius => _aoeRadius;

        public float AoeDuration => _aoeDuration;

        public float ChainRadius => _chainRadius;

        public float ChainDuration => _chainDuration;

        public float ChainAmount => _chainAmount;

        public TargetingPriorityType TargetingPriorityType1 => _targetingPriorityType;

        public float Cooldown => _cooldown;
        public float CastTime => _castTime;
        public List<StatEffectConfig> StatusEffectConfigs => _statusEffectConfigs;

        public int ObjectId => _abilityId;
        public int ConfigTypeId => Constant.DataId.ABILITY_DATA_ID;
        public bool DoExitEffects => _doExitEffects;
        public List<StatEffectConfig> OnExitStatusEffectConfigs => _doExitEffects? _statusEffectConfigsOnExit : null;
    }

    [System.Serializable]
    public struct EffectOnEntityConfig
    {
        [SerializeField] public PlayableAsset EntryTimeLine;
        [SerializeField] public PlayableAsset LoopTimeLine;
        [SerializeField] public PlayableAsset ExitTimeLine;
        
        [SerializeField] public float EntryTime;
        [SerializeField] public float LoopTime;
        [SerializeField] public float ExitTime;
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