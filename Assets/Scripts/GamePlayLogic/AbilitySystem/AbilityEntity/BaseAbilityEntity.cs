using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;
using UnityEngine.Playables;

namespace Tzipory.Systems.AbilitySystem.AbilityEntity
{
    public abstract class BaseAbilityEntity : BaseGameEntity
    {
        [SerializeField] protected Transform _visualTransform;
        [SerializeField] protected PlayableDirector _renderer;
        
        protected IAbilityExecutor AbilityExecutor;
        protected IEntityTargetAbleComponent Target;

        protected void Init(IEntityTargetAbleComponent target, IAbilityExecutor abilityExecutor)
        {
            Target = target;
            AbilityExecutor = abilityExecutor;
        }

        protected virtual void Cast(IEntityTargetAbleComponent target)
        {
            if (target.EntityInstanceID == AbilityExecutor.Caster.EntityInstanceID)
                return;
            
            AbilityExecutor.Execute(target);
        }
    }
}