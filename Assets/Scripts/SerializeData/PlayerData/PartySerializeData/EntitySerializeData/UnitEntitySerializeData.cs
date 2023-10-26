using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.Helpers.Consts;
using Tzipory.SerializeData.AbilitySystem;
using Tzipory.SerializeData.StatSystemSerializeData;
using Tzipory.Systems.Entity;
using UnityEngine;

namespace Tzipory.SerializeData.PlayerData.Party.Entity
{
    [Serializable]
    public class UnitEntitySerializeData : ISerializeData , IUpdateData<BaseUnitEntity>
    {
        [SerializeField,TabGroup("General"),ReadOnly] private string _entityName;
        [SerializeField,TabGroup("General"),ReadOnly] private int _targetingPriority;
        [SerializeField,TabGroup("StatsId"),ReadOnly] private List<StatSerializeData> _statSerializeDatas;

        [SerializeField, TabGroup("Ability"), ReadOnly] private AbilitySerializeData[] _ability;
        [SerializeField, TabGroup("Ability"), ReadOnly] private AbilityConfig[] _abilityConfigs;//temp

        public string EntityName => _entityName;

        public int TargetingPriority => _targetingPriority;

        public AbilitySerializeData[] Ability => _ability;
        
        [Obsolete("Need to use AbilitySerializeData")]
        public AbilityConfig[] AbilityConfigs => _abilityConfigs;

        public List<StatSerializeData> StatSerializeDatas => _statSerializeDatas;

        public int SerializeTypeId => Constant.DataId.SHAMAN_DATA_ID;
        
        public bool IsInitialization { get; private set; }
        
        public virtual void Init(IConfigFile parameter)
        {
            var baseUnitEntityConfig = (BaseUnitEntityConfig)parameter;
            
            _statSerializeDatas  = new List<StatSerializeData>();
            
            _entityName = baseUnitEntityConfig.name;
            
            _abilityConfigs  = baseUnitEntityConfig.AbilityConfigs;
            
            _targetingPriority = (int)baseUnitEntityConfig.TargetingPriority;

            foreach (var statConfig in baseUnitEntityConfig.StatConfigs)
                _statSerializeDatas.Add(new StatSerializeData(statConfig));

            IsInitialization = true;
        }

        public void UpdateData(BaseUnitEntity data)
        {//may be a lot of memory waste!
           //TODO need to make a update function
        }
    }
}