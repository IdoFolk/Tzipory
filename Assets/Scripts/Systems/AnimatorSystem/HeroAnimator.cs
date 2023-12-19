using System.Linq;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

public class HeroAnimator : IEntityAnimatorComponent
{
    public BaseGameEntity GameEntity { get; }
    public AnimatorController EntityAnimatorController { get; private set; }

    public bool IsInitialization { get; }
    
    private IEntityMovementComponent _entityMovementComponent;
    private IEntityHealthComponent _entityHealthComponent;
    private IEntityCombatComponent _entityCombatComponent;
    private IEntityVisualComponent _entityVisualComponent;
    private IEntityAbilitiesComponent _entityAbilitiesComponent;

    private bool _movementChange;
    private bool _isFlipped;
    private AnimatorComponentConfig _config;
    private Animator _entityAnimator;
    private UnitEntity _unitEntity;
    private ParticleSystem _abilityCastEffect;

    public void Init(BaseGameEntity gameEntity,UnitEntity unitEntity, AnimatorComponentConfig config)
    {
        _config = config;
        _unitEntity = unitEntity;
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
            _abilityCastEffect = Object.Instantiate(_config.AbilityCastAnimationPrefab, _unitEntity.GroundCollider.transform).GetComponent<ParticleSystem>();
        }
    }

    private void DeathAnimation()
    {
        
    }

    private void GetHitAnimation(bool obj)
    {
        
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
        if (_isFlipped)
            _entityAnimator.SetTrigger("Attack_Reverse");
        else
            _entityAnimator.SetTrigger("Attack");
    }

    public void Dispose()
    {
        _entityCombatComponent.OnAttack -= AttackAnimation;
        _entityVisualComponent.OnSpriteFlipX -= FlipAnimations;
        foreach (var ability in _entityAbilitiesComponent.Abilities.Select(keyValuePair => keyValuePair.Value).Where(ability => ability.IsActive))
        {
            ability.OnAbilityCast -= AbilityCastAnimation;
            ability.OnAbilityExecute -= AbilityExecuteAnimation;
        }
    }
}
