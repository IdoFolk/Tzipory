using System;
using Helpers.Consts;
using Sirenix.OdinInspector;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.EntitySystem.Entitys;
using Tzipory.SerializeData.AbilitySystemSerializeData;
using Tzipory.SerializeData.StatSystemSerilazeData;
using UnityEngine;

namespace Tzipory.SerializeData.ShamanSerializeData
{
    [Serializable]
    public class UnitEntitySerializeData : ISerializeData
    {
        [SerializeField,TabGroup("General"),ReadOnly] private string _entityName;
        [SerializeField,TabGroup("General"),ReadOnly] private int _targetingPriority;
        [SerializeField,TabGroup("Stats"),ReadOnly] private StatSerializeData _health;
        [SerializeField,TabGroup("Stats"),ReadOnly] private StatSerializeData _invincibleTime;
        [SerializeField,TabGroup("Stats"),ReadOnly] private StatSerializeData _attackDamage;
        [SerializeField,TabGroup("Stats"),ReadOnly] private StatSerializeData _attackRate;
        [SerializeField,TabGroup("Stats"),ReadOnly] private StatSerializeData _attackRange;
        [SerializeField,TabGroup("Stats"),ReadOnly] private StatSerializeData _targetingRange;
        [SerializeField,TabGroup("Stats"),ReadOnly] private StatSerializeData _critDamage;
        [SerializeField,TabGroup("Stats"),ReadOnly] private StatSerializeData _critChance;
        [SerializeField,TabGroup("Stats"),ReadOnly] private StatSerializeData _movementSpeed;
        
        [SerializeField, TabGroup("Ability"), ReadOnly] private AbilitySerializeData[] _ability;
        [SerializeField, TabGroup("Ability"), ReadOnly] private AbilityConfig[] _abilityConfigs;//temp

        public string EntityName => _entityName;

        public int TargetingPriority => _targetingPriority;

        public AbilitySerializeData[] Ability => _ability;
        
        [Obsolete("Need to use AbilitySerializeData")]
        public AbilityConfig[] AbilityConfigs => _abilityConfigs;

        public StatSerializeData Health => _health;

        public StatSerializeData InvincibleTime => _invincibleTime;

        public StatSerializeData AttackDamage => _attackDamage;

        public StatSerializeData AttackRate => _attackRate;

        public StatSerializeData AttackRange => _attackRange;

        public StatSerializeData TargetingRange => _targetingRange;

        public StatSerializeData CritDamage => _critDamage;

        public StatSerializeData CritChance => _critChance;

        public StatSerializeData MovementSpeed => _movementSpeed;
        
        public int SerializeTypeId => Constant.DataId.SHAMAN_DATA_ID;
        

        public UnitEntitySerializeData(BaseUnitEntityConfig shamanConfig)
        {
            _entityName = shamanConfig.name;
           
            _abilityConfigs  = shamanConfig.AbilityConfigs;
            
            _targetingPriority = (int)shamanConfig.TargetingPriority;
            
            _health = new StatSerializeData(shamanConfig.Health);
            _invincibleTime = new StatSerializeData(shamanConfig.InvincibleTime);
            _attackDamage = new StatSerializeData(shamanConfig.AttackDamage);
            _attackRate = new StatSerializeData(shamanConfig.AttackRate);
            _attackRange = new StatSerializeData(shamanConfig.AttackRange);
            _targetingRange = new StatSerializeData(shamanConfig.TargetingRange);
            _critDamage = new StatSerializeData(shamanConfig.CritDamage);
            _critChance = new StatSerializeData(shamanConfig.CritChance);
            _movementSpeed = new StatSerializeData(shamanConfig.MovementSpeed);
        }

        public UnitEntitySerializeData(BaseUnitEntity shaman)
        {
            _entityName = shaman.name;

            _health = new StatSerializeData(shaman.Health);
            _invincibleTime = new StatSerializeData(shaman.InvincibleTime);
            _attackDamage = new StatSerializeData(shaman.AttackDamage);
            _attackRate = new StatSerializeData(shaman.AttackRate);
            _attackRange = new StatSerializeData(shaman.AttackRange);
            _targetingRange = new StatSerializeData(shaman.TargetingRange);
            _critDamage = new StatSerializeData(shaman.CritDamage);
            _critChance = new StatSerializeData(shaman.CritChance);
            _movementSpeed = new StatSerializeData(shaman.MovementSpeed);
        }
    }
}