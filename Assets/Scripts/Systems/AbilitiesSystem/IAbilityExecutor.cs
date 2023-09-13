using System.Collections.Generic;
using Tzipory.ConfigFiles.PartyConfig.AbilitySystemConfig;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.GameplayLogic.StatusEffectTypes;
using Tzipory.ConfigFiles.WaveSystemConfig.StatSystemSerilazeData;

namespace Tzipory.GameplayLogic.AbilitySystem
{
    public interface IAbilityExecutor
    {
        public AbilityExecuteType AbilityExecuteType { get; }
        public IEntityTargetAbleComponent Caster { get; }
        
        public List<StatusEffectConfig> OnEnterStatusEffects { get; }

        public void Init(IEntityTargetAbleComponent target);
        
        public void Execute(IEntityTargetAbleComponent target);
    }
}