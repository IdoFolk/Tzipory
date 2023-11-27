using System;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Systems.Entity.EntityComponents;

namespace Tzipory.Systems.AbilitySystem
{
    public class InstantAbilityCaster : IAbilityCaster
    {
        public event Action OnCast;
        
        public InstantAbilityCaster(IEntityTargetingComponent entityCasterTargetingComponent,AbilityConfig config)
        {
            
        }
        
        public void Cast(ITargetAbleEntity target, IAbilityExecutor abilityExecutor)
        {
            OnCast?.Invoke();
            abilityExecutor.Execute(target);
        }
    }
}