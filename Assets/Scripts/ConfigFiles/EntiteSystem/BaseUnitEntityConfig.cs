using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.ConfigFiles.EntitySystem.EntityVisual;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.TargetingSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem
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

        public abstract int ObjectId { get; }
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