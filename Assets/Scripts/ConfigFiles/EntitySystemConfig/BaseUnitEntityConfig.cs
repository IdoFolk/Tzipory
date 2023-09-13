using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.VisualSystemConfig;
using Helpers.Consts;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.PartyConfig.AbilitySystemConfig;
using Tzipory.ConfigFiles.PartyConfig.EntitySystemConfig.EntityVisualConfig;
using Tzipory.EntitySystem.TargetingSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.PartyConfig.EntitySystemConfig
{
    public abstract class BaseUnitEntityConfig : ScriptableObject, IConfigFile
    {
        [SerializeField, Tooltip(""), TabGroup("StatsId")]
        protected List<StatConfig> _statConfigs;

        [SerializeField, TabGroup("Abilities")]
        private AbilityConfig[] _abilityConfigs;

        [SerializeField, TabGroup("Visual")] private BaseUnitEntityVisualConfig _unitEntityVisualConfig;
        [SerializeField] private TargetingPriorityType _targetingPriority;

        public List<StatConfig> StatConfigs => _statConfigs;

        public TargetingPriorityType TargetingPriority => _targetingPriority;

        public AbilityConfig[] AbilityConfigs => _abilityConfigs;

        public BaseUnitEntityVisualConfig UnitEntityVisualConfig => _unitEntityVisualConfig;

        public abstract int ConfigObjectId { get; }
        public abstract int ConfigTypeId { get; }

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (_statConfigs.Count == 0)
            {
                _statConfigs.Add(new StatConfig() { _statsId = Constant.StatsId.Health });
                _statConfigs.Add(new StatConfig() { _statsId = Constant.StatsId.InvincibleTime });
                _statConfigs.Add(new StatConfig() { _statsId = Constant.StatsId.AttackDamage });
                _statConfigs.Add(new StatConfig() { _statsId = Constant.StatsId.AttackRate });
                _statConfigs.Add(new StatConfig() { _statsId = Constant.StatsId.TargetingRange });
                _statConfigs.Add(new StatConfig() { _statsId = Constant.StatsId.AttackRange });
                _statConfigs.Add(new StatConfig() { _statsId = Constant.StatsId.CritDamage });
                _statConfigs.Add(new StatConfig() { _statsId = Constant.StatsId.CritChance });
                _statConfigs.Add(new StatConfig() { _statsId = Constant.StatsId.MovementSpeed });
            }
#endif
        }
    }
}