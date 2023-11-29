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

        public bool IsAttckingCore;
        
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
            
            foreach (var targetAbleEntity in _self.EntityTargetingComponent.AvailableTargets)
            {
                if (targetAbleEntity.EntityType == EntityType.Core)
                {
                    _self.EntityTargetingComponent.SetAttackTarget(targetAbleEntity);
                    IsAttckingCore = true;
                }
            }

            if (IsAttckingCore)
            {
                _self.EntityCombatComponent.Attack(_self.EntityTargetingComponent.CurrentTarget);
                _self.EntityHealthComponent.StartDeathSequence();
            }

            if (_currentDecisionInterval < 0)
            {
                if (!_isAttacking)
                {
                    if (Random.Range(0, 100) < _aggroLevel)
                    {
                        if (_self.EntityTargetingComponent.HaveTarget)
                        {
                            _isAttacking  = true;
                            Logger.Log($"{GameEntity.name} InstanceID: {GameEntity.EntityInstanceID} is attacking {_self.EntityTargetingComponent.CurrentTarget.GameEntity.name}",ENEMY_LOG_GROUP);
                        }
                    }
                }

                if (_isAttacking)
                {
                    _self.EntityMovementComponent.SetDestination(_self.EntityTargetingComponent.CurrentTarget.GameEntity.EntityTransform.position, MoveType.Free);//temp!

                    if (Random.Range(0, 100) < _returnLevel +
                        Vector3.Distance(GameEntity.EntityTransform.position,_self.EntityMovementComponent.Destination) ||
                        _self.EntityTargetingComponent.CurrentTarget.EntityHealthComponent.IsEntityDead)
                    {
                        _isAttacking = false;
                        _self.EntityMovementComponent.CanMove = true;
                        Debug.Log($"{GameEntity.name} return to path");
                    }
                }

                _currentDecisionInterval = _baseDecisionInterval;
            }
            
            if (_isAttacking)
            {
                if (!_self.EntityTargetingComponent.HaveTarget)
                {
                    _isAttacking = false;
                    return;
                }//plastr

                if (Vector3.Distance(GameEntity.transform.position, _self.EntityTargetingComponent.CurrentTarget.GameEntity.EntityTransform.position) < _self.EntityCombatComponent.AttackRange.CurrentValue)
                    _self.EntityCombatComponent.Attack(_self.EntityTargetingComponent.CurrentTarget);
            }
            
            _currentDecisionInterval -= GAME_TIME.GameDeltaTime;
        }
    }
}