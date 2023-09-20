using System;
using Tzipory.EntitySystem.EntityComponents;

namespace Tzipory.Systems.AbilitySystem
{
    public interface IAbilityCaster
    {
        public event Action OnCast;
       // public void CancelCast();
        public void Cast(IEntityTargetAbleComponent target,IAbilityExecutor abilityExecutor);
    }
}