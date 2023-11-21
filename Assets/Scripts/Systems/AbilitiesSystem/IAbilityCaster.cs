using System;
using Tzipory.Systems.Entity.EntityComponents;

namespace Tzipory.Systems.AbilitySystem
{
    public interface IAbilityCaster
    {
        public event Action OnCast;
        public void Cast(IEntityTargetAbleComponent target,IAbilityExecutor abilityExecutor);
    }
}