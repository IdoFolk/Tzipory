using System.Collections.Generic;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Systems.Entity.EntityComponents;

namespace Tzipory.Systems.AbilitySystem
{
    public interface IAbilityExecutor
    {
        public AbilityExecuteType AbilityExecuteType { get; }
        public ITargetAbleEntity Caster { get; }
        
        public List<StatEffectConfig> EnterStatusEffects { get; }

        public void Init(ITargetAbleEntity target);
        
        public void Execute(ITargetAbleEntity target);
    }
}