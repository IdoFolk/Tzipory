using System.Collections.Generic;
using Helpers.Consts;
using SerializeData.StatSerializeData;
using Sirenix.OdinInspector;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.EntitySystem.TargetingSystem;
using UnityEngine;

namespace Tzipory.EntitySystem.EntityConfigSystem
{
    public abstract class BaseUnitEntityConfig : ScriptableObject
    {
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatSerializeData _health;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatSerializeData _invincibleTime;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatSerializeData _attackDamage;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatSerializeData _attackRate;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatSerializeData _attackRange;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatSerializeData _targetingRange;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatSerializeData _CritDamage;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatSerializeData _CritChance;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private StatSerializeData _movementSpeed;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private List<Stat> _stats;
        [SerializeField,TabGroup("Abilities")] private List<AbilityConfig> _abilityConfigs;
        [SerializeField,TabGroup("Visual")] private Sprite _sprite;
        [SerializeField,TabGroup("Visual")] private Sprite _icon;
        [SerializeField] private TargetingPriorityType _targetingPriority;
        
        public List<Stat> Stats => _stats;

        public StatSerializeData Health => _health;
        
        public StatSerializeData InvincibleTime => _invincibleTime;

        public StatSerializeData AttackDamage => _attackDamage;

        public StatSerializeData AttackRate => _attackRate;

        public StatSerializeData AttackRange => _attackRange;

        public StatSerializeData TargetingRange => _targetingRange;

        public StatSerializeData CritDamage => _CritDamage;

        public StatSerializeData CritChance => _CritChance;

        public StatSerializeData MovementSpeed => _movementSpeed;

        public TargetingPriorityType TargetingPriority => _targetingPriority;

        public List<AbilityConfig> AbilityConfigs => _abilityConfigs;

        public Sprite Sprite => _sprite;

        public Sprite Icon => _icon;

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