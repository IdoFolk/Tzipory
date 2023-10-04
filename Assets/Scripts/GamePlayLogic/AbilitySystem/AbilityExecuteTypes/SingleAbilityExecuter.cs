using System.Collections.Generic;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Systems.Entity.EntityComponents;

namespace Tzipory.Systems.AbilitySystem.AbilityExecuteTypes
{
    public class SingleAbilityExecuter : IAbilityExecutor
    {
        public AbilityExecuteType AbilityExecuteType { get; }
        public IEntityTargetAbleComponent Caster { get; }
        public List<StatEffectConfig> OnEnterStatusEffects { get; }

        public SingleAbilityExecuter(IEntityTargetAbleComponent caster,AbilityConfig abilityConfig)
        {
            Caster = caster;
            OnEnterStatusEffects = new List<StatEffectConfig>();

           OnEnterStatusEffects.AddRange(abilityConfig.StatusEffectConfigs);
        }
        
        public void Init(IEntityTargetAbleComponent target)
        {
            Execute(target);
        }

        public void Execute(IEntityTargetAbleComponent target)
        {
            foreach (var statusEffect in OnEnterStatusEffects)
                target.StatusHandler.AddStatusEffect(statusEffect);
        }
    }
}