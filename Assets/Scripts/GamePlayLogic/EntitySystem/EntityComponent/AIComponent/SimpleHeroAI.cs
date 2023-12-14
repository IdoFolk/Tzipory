using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Tools.TimeSystem;

namespace Tzipory.GamePlayLogic.EntitySystem.AIComponent
{
    public class SimpleHeroAI : IEntityAIComponent
    {
        public bool IsInitialization { get; private set; }
        public BaseGameEntity GameEntity { get; private set; }

        private UnitEntity _self;
        
        private float _currentDecisionInterval = 0;
        private float _baseDecisionInterval;
        
        public void Init(BaseGameEntity parameter1, UnitEntity parameter2,AIComponentConfig config)
        {
            Init(parameter1);

            _self = parameter2;
            
            _baseDecisionInterval = config.DecisionInterval;
            
            IsInitialization = true;
        }

        private void Init(BaseGameEntity parameter)
        {
            GameEntity = parameter;
        }

        public void UpdateComponent()
        {
            if (_self.EntityTargetingComponent.CurrentTarget == null ||
                _self.EntityTargetingComponent.CurrentTarget.EntityHealthComponent.IsEntityDead)
                _self.EntityTargetingComponent.TrySetNewTarget();
            
            _currentDecisionInterval -= GAME_TIME.GameDeltaTime;
            
            if (_currentDecisionInterval < 0)
            {
                _self.EntityTargetingComponent.TrySetNewTarget();
                _currentDecisionInterval = _baseDecisionInterval;
            }
            
            if (_self.EntityMovementComponent.IsMoving)
            {
                if (_self.EntityAbilitiesComponent.IsCasting)
                    _self.EntityAbilitiesComponent?.CancelCast();
                
                return;
            }
            
            _self.EntityAbilitiesComponent?.CastAbility(_self.EntityTargetingComponent.AvailableTargets);

            if (_self.EntityTargetingComponent.HaveTarget)//temp
                _self.EntityCombatComponent.Attack(_self.EntityTargetingComponent.CurrentTarget);
        }
    }
}