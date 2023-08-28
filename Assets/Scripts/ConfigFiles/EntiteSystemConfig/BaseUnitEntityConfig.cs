using System;
using System.Collections.Generic;
using Helpers.Consts;
using SerializeData.StatSerializeData;
using Sirenix.OdinInspector;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.ConfigFiles;
using Tzipory.EntitySystem.EntityConfigSystem.EntityVisualConfig;
using Tzipory.EntitySystem.TargetingSystem;
using UnityEngine;

namespace Tzipory.EntitySystem.EntityConfigSystem
{
    public abstract class BaseUnitEntityConfig : ScriptableObject, IConfigFile
    {
        [SerializeField, Tooltip(""), TabGroup("Stats")]
        private List<StatConfig> _statConfigs;

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

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (_statConfigs.Count != 0)
                return;

            _statConfigs.Add(new StatConfig() { _stats = Constant.Stats.Health });
            _statConfigs.Add(new StatConfig() { _stats = Constant.Stats.InvincibleTime });
            _statConfigs.Add(new StatConfig() { _stats = Constant.Stats.AttackDamage });
            _statConfigs.Add(new StatConfig() { _stats = Constant.Stats.AttackRate });
            _statConfigs.Add(new StatConfig() { _stats = Constant.Stats.TargetingRange });
            _statConfigs.Add(new StatConfig() { _stats = Constant.Stats.AttackRange });
            _statConfigs.Add(new StatConfig() { _stats = Constant.Stats.CritDamage });
            _statConfigs.Add(new StatConfig() { _stats = Constant.Stats.CritChance });
            _statConfigs.Add(new StatConfig() { _stats = Constant.Stats.MovementSpeed });
#endif
        }
    }
}