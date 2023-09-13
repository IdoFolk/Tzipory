using System;
using Tzipory.EntitySystem.EntityComponents;

namespace Tzipory.GameplayLogic.AbilitySystem
{
    public interface IAbilityCaster
    {
        public event Action OnCast;
       // public void CancelCast();
        public void Cast(IEntityTargetAbleComponent target,IAbilityExecutor abilityExecutor);
    }
}