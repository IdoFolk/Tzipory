using System;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.EntitySystem.EntityComponents;

namespace Tzipory.AbilitiesSystem
{
    public class InstantAbilityCaster : IAbilityCaster
    {
        public event Action OnCast;
       // public AbilityCastType AbilityCastType => AbilityCastType.Instant;
        
        public InstantAbilityCaster(IEntityTargetingComponent entityCasterTargetingComponent,AbilityConfig config)
        {
        }

        
        public void Cast(IEntityTargetAbleComponent target, IAbilityExecutor abilityExecutor)
        {
            OnCast?.Invoke();
            abilityExecutor.Init(target);
        }
    }
}