using System.Collections.Generic;
using Tzipory.ConfigFiles.PartyConfig.AbilitySystemConfig;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.ConfigFiles.VisualSystemConfig;

namespace Tzipory.GameplayLogic.AbilitySystem.AbilityExecuteTypes
{
    public class SingleAbilityExecuter : IAbilityExecutor
    {
        public AbilityExecuteType AbilityExecuteType { get; }
        public IEntityTargetAbleComponent Caster { get; }
        public List<StatusEffectConfig> OnEnterStatusEffects { get; }

        public SingleAbilityExecuter(IEntityTargetAbleComponent caster,AbilityConfig abilityConfig)
        {
            Caster = caster;
            OnEnterStatusEffects = new List<StatusEffectConfig>();

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