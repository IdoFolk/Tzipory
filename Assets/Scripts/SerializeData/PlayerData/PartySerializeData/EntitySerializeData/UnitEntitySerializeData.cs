using System;
using Helpers.Consts;
using Sirenix.OdinInspector;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.ConfigFiles;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.EntitySystem.Entitys;
using Tzipory.SerializeData.AbilitySystemSerializeData;
using Tzipory.SerializeData.StatSystemSerilazeData;
using UnityEngine;

namespace Tzipory.SerializeData
{
    [Serializable]
    public class UnitEntitySerializeData : ISerializeData , IUpdateData<BaseUnitEntity>
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
        
        public bool IsInitialization { get; private set; }
        
        public virtual void Init(IConfigFile parameter)
        {
            var baseUnitEntityConfig = (BaseUnitEntityConfig)parameter;
            
            _entityName = baseUnitEntityConfig.name;
            
            _abilityConfigs  = baseUnitEntityConfig.AbilityConfigs;
            
            _targetingPriority = (int)baseUnitEntityConfig.TargetingPriority;
            
            _health = new StatSerializeData(baseUnitEntityConfig.Health);
            _invincibleTime = new StatSerializeData(baseUnitEntityConfig.InvincibleTime);
            _attackDamage = new StatSerializeData(baseUnitEntityConfig.AttackDamage);
            _attackRate = new StatSerializeData(baseUnitEntityConfig.AttackRate);
            _attackRange = new StatSerializeData(baseUnitEntityConfig.AttackRange);
            _targetingRange = new StatSerializeData(baseUnitEntityConfig.TargetingRange);
            _critDamage = new StatSerializeData(baseUnitEntityConfig.CritDamage);
            _critChance = new StatSerializeData(baseUnitEntityConfig.CritChance);
            _movementSpeed = new StatSerializeData(baseUnitEntityConfig.MovementSpeed);
            
            IsInitialization = true;
        }

        public void UpdateData(BaseUnitEntity data)
        {//may be a lot of memory waste!
            _health.UpdateData(data.Health);
            _invincibleTime.UpdateData(data.InvincibleTime);
            _attackDamage.UpdateData(data.AttackDamage);
            _attackRate.UpdateData(data.AttackRate);
            _attackRange.UpdateData(data.AttackRange);
            _targetingRange.UpdateData(data.TargetingRange);
            _critDamage.UpdateData(data.CritDamage);
            _critChance.UpdateData(data.CritChance);
            _movementSpeed.UpdateData(data.MovementSpeed);
        }
    }
}