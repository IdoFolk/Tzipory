using Tzipory.Systems.Entity;
using Tzipory.Systems.VisualSystem;
using UnityEngine;

namespace Tzipory.Systems.AbilitySystem.AbilityEntity
{
    public abstract class BaseAbilityEntity : BaseGameEntity
    {
        [SerializeField] protected Transform _visualTransform;

        [SerializeField] private AbilityVisualHandler _abilityVisualHandler;
        
        protected IAbilityExecutor AbilityExecutor;

        protected void Init(IAbilityExecutor abilityExecutor)
        {
            AbilityExecutor = abilityExecutor;
        }
    }
}