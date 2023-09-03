using System;
using System.Collections.Generic;
using Helpers.Consts;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.EntitySystem.TargetingSystem;
using UnityEngine;

namespace Tzipory.AbilitiesSystem
{
    public class Ability : IStatHolder
    {
        private readonly IEntityTargetingComponent _entityTargetingComponent;
        private readonly IAbilityCaster _abilityCaster;
        private readonly IAbilityExecutor _abilityExecutor;
        private readonly IPriorityTargeting _priorityTargeting;
        
        private bool _isReady;
        
        private ITimer _castTimer;
        private ITimer _cooldownTimer;

        public string AbilityName { get; }
        public int AbilityId { get; }
        public bool IsCasting { get; private set; }
        
        public Dictionary<int, Stat> Stats { get; }

        private Stat Cooldown
        {
            get
            {
                if (Stats.TryGetValue((int)Constant.Stats.AbilityCooldown, out var coolDown))
                    return coolDown;
                
                throw new Exception($"Cooldown not found on ability {AbilityName} in entity {_entityTargetingComponent.GameEntity.name}");
            }
        }
        private Stat CastTime 
        {
            get
            {
                if (Stats.TryGetValue((int)Constant.Stats.AbilityCastTime, out var castTime))
                    return castTime;
                
                throw new Exception($"CastTime not found on ability {AbilityName} in entity {_entityTargetingComponent.GameEntity.name}");
            }
        }
        
        [Obsolete("Use AbilitySerializeData")]
        public Ability(IEntityTargetAbleComponent caster,IEntityTargetingComponent entityTargetingComponent, AbilityConfig config)
        {
            _entityTargetingComponent = entityTargetingComponent;

            Stats = new Dictionary<int, Stat>();
            
            AbilityName = config.AbilityName;
            AbilityId = config.AbilityId;
            
            Stats.Add((int)Constant.Stats.AbilityCooldown,new Stat(Constant.Stats.AbilityCooldown.ToString(), config.Cooldown, int.MaxValue,
                (int)Constant.Stats.AbilityCooldown));
            Stats.Add((int)Constant.Stats.AbilityCastTime,new Stat(Constant.Stats.AbilityCastTime.ToString(), config.CastTime, int.MaxValue,
                (int)Constant.Stats.AbilityCastTime));
            

            _abilityCaster = Factory.AbilityFactory.GetAbilityCaster(entityTargetingComponent,config);
            _abilityExecutor = Factory.AbilityFactory.GetAbilityExecutor(caster,config);

            _abilityCaster.OnCast += StartCooldown;
            
            _priorityTargeting =
                Factory.TargetingPriorityFactory.GetTargetingPriority(entityTargetingComponent,
                    config.TargetingPriorityType);

            _isReady = true;
        }
        
        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            List<IStatHolder> statHolders = new List<IStatHolder> { this };

            if (_abilityCaster is IStatHolder abilityCaster)
                statHolders.AddRange(abilityCaster.GetNestedStatHolders());
            
            if (_abilityExecutor is IStatHolder abilityExecutor)
                statHolders.AddRange(abilityExecutor.GetNestedStatHolders());

            return statHolders;
        }

        public void ExecuteAbility(IEnumerable<IEntityTargetAbleComponent> availableTarget)
        {
            if (!_isReady)
                return;

            _isReady = false;
            IsCasting = true;
#if UNITY_EDITOR
            Debug.Log($"<color=#0008ff>AbilityHandler:</color> {_entityTargetingComponent.GameEntity.name} start casting ability {AbilityName} castTime: {CastTime.CurrentValue}");
            
#endif
            _castTimer = _entityTargetingComponent.GameEntity.EntityTimer.StartNewTimer(CastTime.CurrentValue,"Ability cast time", Cast,ref availableTarget);
        }

        private void Cast(IEnumerable<IEntityTargetAbleComponent> availableTarget)
        {
            var currentTarget = _priorityTargeting.GetPriorityTarget(availableTarget);
            
            if (currentTarget == null)
                return;
#if UNITY_EDITOR
            Debug.Log($"<color=#0008ff>AbilityHandler:</color> {_entityTargetingComponent.GameEntity.name} cast ability {AbilityName} on {currentTarget.GameEntity.name}");
#endif
            _abilityCaster.Cast(currentTarget,_abilityExecutor);
        }
        
        public void CancelCast()
        {
#if UNITY_EDITOR
            Debug.Log($"<color=#0008ff>AbilityHandler:</color> {_entityTargetingComponent.GameEntity.name} ability {AbilityName} cancel cast {_castTimer.TimeRemaining}");
#endif
            _castTimer.StopTimer();
            IsCasting = false;
            _isReady = true;
        }

        private void StartCooldown()
        {
            IsCasting  = false;
            _entityTargetingComponent.GameEntity.EntityTimer.StartNewTimer(Cooldown.CurrentValue,"Ability cooldown",ResetAbility);
        }

        private void ResetAbility() =>
            _isReady = true;

        ~Ability()
        {
            _abilityCaster.OnCast -= StartCooldown;
        }
    }
}