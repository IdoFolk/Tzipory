using Tzipory.EntitySystem;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

namespace Tzipory.GameplayLogic.AbilitySystem.AbilityEntity
{
    public abstract class BaseAbilityEntity : BaseGameEntity
    {
        [SerializeField] protected Transform visualTransform;
        
        protected IAbilityExecutor _abilityExecutor;
        protected IEntityTargetAbleComponent _target;

        protected void Init(IEntityTargetAbleComponent target, IAbilityExecutor abilityExecutor)
        {
            _target = target;
            _abilityExecutor = abilityExecutor;
        }

        protected virtual void Cast(IEntityTargetAbleComponent target)
        {
            if (target.EntityInstanceID == _abilityExecutor.Caster.EntityInstanceID)
                return;
            
            _abilityExecutor.Execute(target);
        }
    }
}