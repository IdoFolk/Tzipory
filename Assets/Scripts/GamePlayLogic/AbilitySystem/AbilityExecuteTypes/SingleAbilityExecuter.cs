using System.Collections.Generic;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.StatusSystem;

namespace Tzipory.AbilitiesSystem.AbilityExecuteTypes
{
    public class SingleAbilityExecuter : IAbilityExecutor
    {
        public AbilityExecuteType AbilityExecuteType { get; }
        public IEntityTargetAbleComponent Caster { get; }
        public List<StatusEffectConfig> StatusEffects { get; }

        public SingleAbilityExecuter(IEntityTargetAbleComponent caster,AbilityConfig abilityConfig)
        {
            Caster = caster;
            StatusEffects = new List<StatusEffectConfig>();

           StatusEffects.AddRange(abilityConfig.StatusEffectConfigs);
        }
        
        public void Init(IEntityTargetAbleComponent target)
        {
            Execute(target);
        }

        public void Execute(IEntityTargetAbleComponent target)
        {
            foreach (var statusEffect in StatusEffects)
                target.StatusHandler.AddStatusEffect(statusEffect);
        }
    }
}