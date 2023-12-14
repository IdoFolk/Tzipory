using System.Collections.Generic;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Tools.Interface;

namespace Tzipory.Systems.AbilitySystem.AbilityExecuteTypes
{
    public class StatEffectExecuter :  IAbilityExecutor , IInitialization<IEnumerable<StatEffectConfig>>
    {
        private IEnumerable<StatEffectConfig> _statEffectConfig;
        
        public bool IsInitialization { get; private set; }
        
        public void Init(IEnumerable<StatEffectConfig>  parameter)
        {
            _statEffectConfig = parameter;

            IsInitialization = true;
        }
        
        public void Execute(ITargetAbleEntity target)
        {
            foreach (var statEffectConfig in _statEffectConfig)
                target.EntityStatComponent.AddStatEffect(statEffectConfig);
        }
    }
}