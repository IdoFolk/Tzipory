using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using Logger = Tzipory.Tools.Debag.Logger;

namespace Tzipory.GamePlayLogic.EntitySystem.AIComponent
{
    public class SimpleEnemyAI : IEntityAIComponent
    {
        private const string ENEMY_LOG_GROUP = "Enemy";
        
        public bool IsInitialization { get; private set; }
        public BaseGameEntity GameEntity { get; private set; }

        private UnitEntity _self;
        
        private float _currentDecisionInterval = 0;
        private float _baseDecisionInterval;
        
        private float _aggroLevel;//temp
        private float _returnLevel;//temp
        
        private bool _isAttacking;
        
        public void Init(BaseGameEntity parameter1, UnitEntity parameter2,AIComponentConfig config)
        {
            Init(parameter1);

            _self = parameter2;
            
            _baseDecisionInterval = config.DecisionInterval;
            
            _aggroLevel = config.AggroLevel;
            _returnLevel = config.ReturnLevel;
            
            IsInitialization = true;
        }

        public void Init(BaseGameEntity parameter)
        {
            GameEntity = parameter;
        }

        public void UpdateComponent()
        {
            if (!IsInitialization)
                return;
            
            if (_currentDecisionInterval  < 0)
                _currentDecisionInterval = _baseDecisionInterval;
            
            _currentDecisionInterval -= GAME_TIME.GameDeltaTime;
            
            if (!_self.EntityTargetingComponent.HaveTarget)
            {
                if (_self.EntityTargetingComponent.HaveTargetInRange)
                {
                    _self.EntityTargetingComponent.TrySetNewTarget();
                }
                _self.EntityMovementComponent.CanMove = true;
                return;
            }
            
            if (_currentDecisionInterval < 0 && _self.EntityTargetingComponent.HaveTarget)
            {
                if (_self.EntityTargetingComponent.CurrentTarget.EntityType == EntityType.Core) return;
                
                if (!_isAttacking)
                {
                    if (Random.Range(0, 100) < _aggroLevel)
                    {
                        _isAttacking  = true;
                        _self.EntityMovementComponent.CanMove = false;
                        Logger.Log($"{GameEntity.name} InstanceID: {GameEntity.EntityInstanceID} is attacking {_self.EntityTargetingComponent.CurrentTarget.GameEntity.name}",ENEMY_LOG_GROUP);
                    }
                }
                
                if (_isAttacking && Random.Range(0, 100) < _returnLevel ||
                    _self.EntityTargetingComponent.CurrentTarget.EntityHealthComponent.IsEntityDead)
                {
                    _isAttacking = false;
                    _self.EntityMovementComponent.CanMove = true;
                    Logger.Log($"{GameEntity.name} return to path",ENEMY_LOG_GROUP);
                }
            }
            
            if (_isAttacking && _self.EntityTargetingComponent.CurrentTarget is not null)
            {
                _self.EntityMovementComponent.SetDestination(_self.EntityTargetingComponent.CurrentTarget.GameEntity.EntityTransform.position, MoveType.Free);//temp!
                    
                if (Vector3.Distance(GameEntity.transform.position, _self.EntityTargetingComponent.CurrentTarget.GameEntity.EntityTransform.position) < _self.EntityCombatComponent.AttackRange.CurrentValue)
                    _self.EntityCombatComponent.Attack(_self.EntityTargetingComponent.CurrentTarget);
            }
            
            if (!_self.EntityTargetingComponent.HaveTarget)
            {
                _isAttacking = false;
                _self.EntityMovementComponent.CanMove = true;
            }//plastr
        }
    }
}