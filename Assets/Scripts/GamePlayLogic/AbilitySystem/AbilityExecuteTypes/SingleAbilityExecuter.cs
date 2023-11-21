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
        public List<StatEffectConfig> EnterStatusEffects { get; }

        public SingleAbilityExecuter(IEntityTargetAbleComponent caster,AbilityConfig abilityConfig)
        {
            Caster = caster;
            EnterStatusEffects = new List<StatEffectConfig>();

           EnterStatusEffects.AddRange(abilityConfig.StatusEffectConfigs);
        }

        public void Init(IEntityTargetAbleComponent target)
        {
        }

        public void Execute(IEntityTargetAbleComponent target)
        {
            foreach (var statusEffect in EnterStatusEffects)
                target.StatHandler.AddStatEffect(statusEffect);
        }
    }
}