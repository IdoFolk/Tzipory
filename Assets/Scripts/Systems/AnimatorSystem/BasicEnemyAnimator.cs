using System;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;

public class BasicEnemyAnimator : IEntityAnimatorComponent
{
    public BaseGameEntity GameEntity { get; }
    public RuntimeAnimatorController EntityAnimatorController { get; private set; }

    public bool IsInitialization { get; }
    
    private IEntityMovementComponent _entityMovementComponent;
    private IEntityHealthComponent _entityHealthComponent;
    private IEntityCombatComponent _entityCombatComponent;
    private IEntityVisualComponent _entityVisualComponent;
    
    private Animator _entityAnimator;

    private bool _isFlipped;
    private bool _movementChange;
    
    public void Init(BaseGameEntity gameEntity, UnitEntity unitEntity, AnimatorComponentConfig config)
    {
        EntityAnimatorController = config.EntityAnimator;
        _entityAnimator = unitEntity.EntityAnimator;
        
        _entityMovementComponent = gameEntity.RequestComponent<IEntityMovementComponent>();
        _entityHealthComponent = gameEntity.RequestComponent<IEntityHealthComponent>();
        _entityCombatComponent = gameEntity.RequestComponent<IEntityCombatComponent>();
        _entityVisualComponent = gameEntity.RequestComponent<IEntityVisualComponent>();
        
        _entityHealthComponent.OnHit += GetHitAnimation;
        _entityHealthComponent.OnDeath += DeathAnimation;
        _entityCombatComponent.OnAttack += AttackAnimation; 
        _entityVisualComponent.OnSpriteFlipX += FlipAnimations;
    }

    private void FlipAnimations(bool state)
    {
        _isFlipped = state;
    }

    private void AttackAnimation()
    {
        _entityAnimator.SetTrigger(_isFlipped ? "Attack_Flipped" : "Attack");
    }

    private void DeathAnimation()
    {
        _entityMovementComponent.CanMove = false;
        _entityAnimator.SetBool("Dead",true);
        _entityAnimator.SetTrigger(_isFlipped ? "Death_Flipped" : "Death");
    }

    private void GetHitAnimation(bool isCrit)
    {
        _entityAnimator.SetTrigger(_isFlipped ? "GetHit_Flipped" : "GetHit");
    }

    public void UpdateComponent()
    {
        // if (_entityMovementComponent.IsMoving != _movementChange)
        // {
        //     _movementChange = _entityMovementComponent.IsMoving; 
        //     _entityAnimator.SetBool("Walking", _movementChange);
        // }
    }

    public void Dispose()
    {
        _entityHealthComponent.OnHit -= GetHitAnimation;
        _entityHealthComponent.OnDeath -= DeathAnimation;
        _entityCombatComponent.OnAttack -= AttackAnimation; 
        _entityVisualComponent.OnSpriteFlipX -= FlipAnimations;
    }
}
