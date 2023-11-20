using System.Collections.Generic;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Systems.Entity.EntityComponents;

namespace Tzipory.Systems.AbilitySystem
{
    public interface IAbilityExecutor
    {
        public AbilityExecuteType AbilityExecuteType { get; }
        public IEntityTargetAbleComponent Caster { get; }
        
        public List<StatEffectConfig> EnterStatusEffects { get; }

        public void Init(IEntityTargetAbleComponent target);
        
        public void Execute(IEntityTargetAbleComponent target);
    }
}