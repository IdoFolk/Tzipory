using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Tools.TimeSystem;
using Logger = Tzipory.Tools.Debag.Logger;

namespace Tzipory.Systems.AbilitySystem
{
    public class Ability : IStatHolder
    {
        private const string ABILITY_LOG_GROUP = "AbilityHandler";
        
        private readonly IEntityTargetingComponent _entityTargetingComponent;
        private readonly IAbilityExecutor _abilityExecutor;
        private readonly IPriorityTargeting _priorityTargeting;

        private bool _isReady;


        private ITimer _castTimer;
        private ITimer _cooldownTimer;

        public string AbilityName { get; }
        public int AbilityId { get; }
        public bool IsCasting { get; private set; }
        public bool IsActive { get; private set; }
        public event Action<int> OnAbilityCast;
        public event Action<int> OnAbilityExecute;
        public Dictionary<int, Stat> Stats { get; }

        public AbilityConfig Config { get; private set; }

        public float CooldownTimeRemaining
        {
            get
            {
                if (_cooldownTimer is null) return 0;
                return _cooldownTimer.TimeRemaining;
            }
        }

        private Stat Cooldown
        {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.AbilityCooldown, out var coolDown))
                    return coolDown;
                
                throw new Exception($"Cooldown not found on ability {AbilityName} in entity {_entityTargetingComponent.GameEntity.name}");
            }
        }
        private Stat CastTime 
        {
            get
            {
                if (Stats.TryGetValue((int)Constant.StatsId.AbilityCastTime, out var castTime))
                    return castTime;
                
                throw new Exception($"CastTime not found on ability {AbilityName} in entity {_entityTargetingComponent.GameEntity.name}");
            }
        }
        
        [Obsolete("Use AbilitySerializeData")]
        public Ability(ITargetAbleEntity caster,IEntityTargetingComponent entityTargetingComponent, AbilityConfig config)
        {
            _entityTargetingComponent = entityTargetingComponent;
            Config = config;

            Stats = new Dictionary<int, Stat>();
            
            AbilityName = config.AbilityName;
            AbilityId = config.AbilityId;
            
            Stats.Add((int)Constant.StatsId.AbilityCooldown,new Stat(Constant.StatsId.AbilityCooldown.ToString(), config.Cooldown, int.MaxValue,
                (int)Constant.StatsId.AbilityCooldown));
            Stats.Add((int)Constant.StatsId.AbilityCastTime,new Stat(Constant.StatsId.AbilityCastTime.ToString(), config.CastTime, int.MaxValue,
                (int)Constant.StatsId.AbilityCastTime));
            
            _abilityExecutor = FactorySystem.ObjectFactory.AbilityFactory.GetAbilityExecutor(caster,config);
            
            _priorityTargeting =
                FactorySystem.ObjectFactory.TargetingPriorityFactory.GetTargetingPriority(entityTargetingComponent,
                    config.TargetingPriorityType);

            _isReady = true;
            IsActive = true;
        }
        
        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            List<IStatHolder> statHolders = new List<IStatHolder> { this };
            
            if (_abilityExecutor is IStatHolder abilityExecutor)
                statHolders.AddRange(abilityExecutor.GetNestedStatHolders());

            return statHolders;
        }

        public void ExecuteAbility(IEnumerable<ITargetAbleEntity> availableTarget)
        {
            var currentTarget = _priorityTargeting.GetPriorityTarget(availableTarget);
            
            if (!_isReady)
                return;
            
            if (currentTarget == null)
            {
                IsCasting = false;
                return;
            }
            _isReady = false;
            IsCasting = true;
            
            Logger.Log($"{_entityTargetingComponent.GameEntity.name} start casting ability {AbilityName} castTime: {CastTime.CurrentValue}",ABILITY_LOG_GROUP);
            
            _castTimer = _entityTargetingComponent.GameEntity.EntityTimer.StartNewTimer(CastTime.CurrentValue,"Ability cast time",Cast,ref availableTarget);
            OnAbilityCast?.Invoke(AbilityId);
        }

        private void Cast(IEnumerable<ITargetAbleEntity> availableTarget)
        {
            var currentTarget = _priorityTargeting.GetPriorityTarget(availableTarget);

            if (currentTarget == null)
            {
                IsCasting = false;
                _isReady = true;
                return;
            }
            
            Logger.Log($"{_entityTargetingComponent.GameEntity.name} cast ability {AbilityName} on {currentTarget.GameEntity.name}",ABILITY_LOG_GROUP);
            _abilityExecutor.Execute(currentTarget);
            OnAbilityExecute?.Invoke(AbilityId);
            StartCooldown();
        }
        
        public void CancelCast()
        {
            Logger.Log($"{_entityTargetingComponent.GameEntity.name} ability {AbilityName} cancel cast {_castTimer.TimeRemaining}",ABILITY_LOG_GROUP);
            _castTimer.StopTimer();
            IsCasting = false;
            _isReady = true;
        }

        private void StartCooldown()
        {
            IsCasting  = false;
            _cooldownTimer = _entityTargetingComponent.GameEntity.EntityTimer.StartNewTimer(Cooldown.CurrentValue,"Ability cooldown",ResetAbility);
        }

        private void ResetAbility() =>
            _isReady = true;
    }
}