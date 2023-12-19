using System;
using System.Linq;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using Object = UnityEngine.Object;

public class HeroAnimator : IEntityAnimatorComponent
{
    public BaseGameEntity GameEntity { get; }
    public RuntimeAnimatorController EntityAnimatorController { get; private set; }

    public bool IsInitialization { get; }
    
    private IEntityMovementComponent _entityMovementComponent;
    private IEntityHealthComponent _entityHealthComponent;
    private IEntityCombatComponent _entityCombatComponent;
    private IEntityVisualComponent _entityVisualComponent;
    private IEntityAbilitiesComponent _entityAbilitiesComponent;

    private bool _movementChange;
    private bool _isFlipped;
    private Animator _entityAnimator;
    private ParticleSystem _abilityCastEffect;

    public void Init(BaseGameEntity gameEntity,UnitEntity unitEntity, AnimatorComponentConfig config)
    {
        _entityAnimator = unitEntity.EntityAnimator;
        EntityAnimatorController = config.EntityAnimator;
        unitEntity.EntityAnimator.runtimeAnimatorController = EntityAnimatorController;
        
        _entityMovementComponent = gameEntity.RequestComponent<IEntityMovementComponent>();
        _entityCombatComponent = gameEntity.RequestComponent<IEntityCombatComponent>();
        _entityVisualComponent = gameEntity.RequestComponent<IEntityVisualComponent>();
        _entityAbilitiesComponent = gameEntity.RequestComponent<IEntityAbilitiesComponent>();
        _entityHealthComponent = gameEntity.RequestComponent<IEntityHealthComponent>();

        _entityCombatComponent.OnAttack += AttackAnimation; 
        _entityVisualComponent.OnSpriteFlipX += FlipAnimations;
        _entityHealthComponent.OnHit += GetHitAnimation;
        _entityHealthComponent.OnDeath += DeathAnimation;
        foreach (var ability in _entityAbilitiesComponent.Abilities.Select(keyValuePair => keyValuePair.Value).Where(ability => ability.IsActive))
        {
            ability.OnAbilityCast += AbilityCastAnimation;
            ability.OnAbilityExecute += AbilityExecuteAnimation;
        }

        if (config.AbilityCastAnimationPrefab is not null)
        {
            _abilityCastEffect = Object.Instantiate(config.AbilityCastAnimationPrefab, unitEntity.GroundCollider.transform).GetComponent<ParticleSystem>();
        }
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

    private void AbilityExecuteAnimation(int abilityId)
    {
        _abilityCastEffect.Stop();
    }

    private void AbilityCastAnimation(int abilityId)
    {
        _abilityCastEffect.Play();
    }

    private void FlipAnimations(bool state)
    {
        _isFlipped = state;
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
        _entityAnimator.SetTrigger(_isFlipped ? "Attack_Flipped" : "Attack");
    }

    public void Dispose()
    {
        _entityCombatComponent.OnAttack -= AttackAnimation;
        _entityVisualComponent.OnSpriteFlipX -= FlipAnimations;
        _entityHealthComponent.OnHit -= GetHitAnimation;
        _entityHealthComponent.OnDeath -= DeathAnimation;
        foreach (var ability in _entityAbilitiesComponent.Abilities.Select(keyValuePair => keyValuePair.Value).Where(ability => ability.IsActive))
        {
            ability.OnAbilityCast -= AbilityCastAnimation;
            ability.OnAbilityExecute -= AbilityExecuteAnimation;
        }
    }
}
