using System;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.EntitySystem.EntityComponents;

namespace Tzipory.AbilitiesSystem
{
    public interface IAbilityCaster
    {
        public event Action OnCast;
       // public void CancelCast();
        public void Cast(IEntityTargetAbleComponent target,IAbilityExecutor abilityExecutor);
    }
}