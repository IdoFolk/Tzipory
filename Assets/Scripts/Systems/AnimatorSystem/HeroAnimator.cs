using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEditor.Animations;
using UnityEngine;

public class HeroAnimator : IEntityAnimatorComponent
{
    public BaseGameEntity GameEntity { get; }
    public AnimatorController EntityAnimatorController { get; private set; }

    public bool IsInitialization { get; }
    
    private IEntityMovementComponent _entityMovementComponent;
    private IEntityCombatComponent _entityCombatComponent;

    private bool _movementChange;
    private Animator _entityAnimator;

    public void Init(BaseGameEntity parameter1,UnitEntity parameter2, AnimatorComponentConfig parameter3)
    {
        _entityAnimator = parameter2.EntityAnimator;
        EntityAnimatorController = parameter3.EntityAnimator;
        parameter2.EntityAnimator.runtimeAnimatorController = EntityAnimatorController;
        
        _entityMovementComponent = parameter1.RequestComponent<IEntityMovementComponent>();
        _entityCombatComponent = parameter1.RequestComponent<IEntityCombatComponent>();

        _entityCombatComponent.OnAttack += AttackAnimation; //add unsubscribe!
    }

    public void UpdateComponent()
    {
        if (_entityMovementComponent.IsMoving != _movementChange)
        {
            _movementChange = _entityMovementComponent.IsMoving;
            _entityAnimator.SetBool("Walking", _movementChange);
        }
    }
    
    private void AttackAnimation()
    {
        _entityAnimator.SetTrigger("Attack");
    }
}
