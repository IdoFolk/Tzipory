using System.Collections.Generic;
using Helpers.Consts;
using Sirenix.OdinInspector;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.EntitySystem.TargetingSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tzipory.EntitySystem.EntityConfigSystem
{
    //Alon TBC - is this ok? is there a rule/convention that I maybe missed here?
    public abstract class BaseUnitEntityConfig : ScriptableObject
    {
        [SerializeField,Tooltip(""),TabGroup("Stats")] private Stat _health;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private Stat _invincibleTime;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private Stat _attackDamage;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private Stat _AttackRate;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private Stat _attackRange;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private Stat _targetingRange;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private Stat _CritDamage;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private Stat _CritChance;
        [FormerlySerializedAs("_moveSpeed")] [SerializeField,Tooltip(""),TabGroup("Stats")] private Stat _movementSpeed;
        [SerializeField,Tooltip(""),TabGroup("Stats")] private List<Stat> _stats;
        [SerializeField,TabGroup("Abilities")] private List<AbilityConfig> _abilityConfigs;
        [SerializeField,TabGroup("Visual")] private Sprite _sprite;
        [SerializeField,TabGroup("Visual")] private Sprite _icon;
        [SerializeField] private TargetingPriorityType _targetingPriority;
        
        public List<Stat> Stats => _stats;

        public Stat Health => _health;
        
        public Stat InvincibleTime => _invincibleTime;

        public Stat AttackDamage => _attackDamage;

        public Stat AttackRate => _AttackRate;

        public Stat AttackRange => _attackRange;

        public Stat TargetingRange => _targetingRange;

        public Stat CritDamage => _CritDamage;

        public Stat CritChance => _CritChance;

        public Stat MovementSpeed => _movementSpeed;

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
            _AttackRate.Name =     Constant.Stats.AttackRate.ToString();
            _targetingRange.Name = Constant.Stats.TargetingRange.ToString();
            _attackRange.Name =    Constant.Stats.AttackRange.ToString();
            _CritDamage.Name =     Constant.Stats.CritDamage.ToString();
            _CritChance.Name =     Constant.Stats.CritChance.ToString();
            _movementSpeed.Name =  Constant.Stats.MovementSpeed.ToString();

            _health.Id =         (int)Constant.Stats.Health;
            _invincibleTime.Id = (int)Constant.Stats.InvincibleTime;
            _attackDamage.Id =   (int)Constant.Stats.AttackDamage;
            _AttackRate.Id =     (int)Constant.Stats.AttackRate;
            _targetingRange.Id = (int)Constant.Stats.TargetingRange;
            _attackRange.Id =    (int)Constant.Stats.AttackRange;
            _CritDamage.Id =     (int)Constant.Stats.CritDamage;
            _CritChance.Id =     (int)Constant.Stats.CritChance;
            _movementSpeed.Id =  (int)Constant.Stats.MovementSpeed;
#endif
        }
    }
}