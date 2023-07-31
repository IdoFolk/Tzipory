using System.Collections.Generic;
using Helpers.Consts;
using SerializeData.StatSerializeData;
using Sirenix.OdinInspector;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.ConfigFiles;
using Tzipory.EntitySystem.EntityConfigSystem.EntityVisualConfig;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.EntitySystem.TargetingSystem;
using UnityEngine;

namespace Tzipory.EntitySystem.EntityConfigSystem
{
    public abstract class BaseUnitEntityConfig : ScriptableObject , IConfigFile
    {
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatConfig _health;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatConfig _invincibleTime;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatConfig _attackDamage;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatConfig _attackRate;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatConfig _attackRange;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatConfig _targetingRange;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatConfig _CritDamage;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatConfig _CritChance;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatConfig _movementSpeed;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private List<Stat> _stats;
        [SerializeField,TabGroup("Abilities")] private AbilityConfig[] _abilityConfigs;
        [SerializeField,TabGroup("Visual")] private BaseUnitEntityVisualConfig _unitEntityVisualConfig;
        [SerializeField] private TargetingPriorityType _targetingPriority;
        
        public List<Stat> Stats => _stats;

        public StatConfig Health => _health;
        
        public StatConfig InvincibleTime => _invincibleTime;

        public StatConfig AttackDamage => _attackDamage;

        public StatConfig AttackRate => _attackRate;

        public StatConfig AttackRange => _attackRange;

        public StatConfig TargetingRange => _targetingRange;

        public StatConfig CritDamage => _CritDamage;

        public StatConfig CritChance => _CritChance;

        public StatConfig MovementSpeed => _movementSpeed;

        public TargetingPriorityType TargetingPriority => _targetingPriority;

        public AbilityConfig[] AbilityConfigs => _abilityConfigs;

        public BaseUnitEntityVisualConfig UnitEntityVisualConfig => _unitEntityVisualConfig;

        public abstract int ConfigObjectId { get; }
        public abstract int ConfigTypeId { get; }
        
        private void OnValidate()
        {
#if UNITY_EDITOR
            _health.Name =         Constant.Stats.Health.ToString();
            _invincibleTime.Name = Constant.Stats.InvincibleTime.ToString();
            _attackDamage.Name =   Constant.Stats.AttackDamage.ToString();
            _attackRate.Name =     Constant.Stats.AttackRate.ToString();
            _targetingRange.Name = Constant.Stats.TargetingRange.ToString();
            _attackRange.Name =    Constant.Stats.AttackRange.ToString();
            _CritDamage.Name =     Constant.Stats.CritDamage.ToString();
            _CritChance.Name =     Constant.Stats.CritChance.ToString();
            _movementSpeed.Name =  Constant.Stats.MovementSpeed.ToString();

            _health.Id =           (int)Constant.Stats.Health;
            _invincibleTime.Id =   (int)Constant.Stats.InvincibleTime;
            _attackDamage.Id =     (int)Constant.Stats.AttackDamage;
            _attackRate.Id =       (int)Constant.Stats.AttackRate;
            _targetingRange.Id =   (int)Constant.Stats.TargetingRange;
            _attackRange.Id =      (int)Constant.Stats.AttackRange;
            _CritDamage.Id =       (int)Constant.Stats.CritDamage;
            _CritChance.Id =       (int)Constant.Stats.CritChance;
            _movementSpeed.Id =    (int)Constant.Stats.MovementSpeed;
#endif
        }
    }
}