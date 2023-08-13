using System;
using Helpers.Consts;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.EntitySystem.Entitys;
using Tzipory.Systems.PoolSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemes
{
    public class Enemy : BaseUnitEntity , IPoolable<Enemy>
    {
        private float _decisionInterval;//temp
        private float _aggroLevel;//temp
        private float _returnLevel;//temp
        
        private bool _isAttacking;

        private float _currentDecisionInterval;

        public bool IsAttckingCore;
        
        //TEMP!
        [SerializeField] private MovementOnPath _movementOnPath;
        public TEMP_BasicMoveComponent _tempBasicMoveComponent;

        float timer;
        
        public override void Init(BaseUnitEntityConfig parameter)
        {
            base.Init(parameter);
            
            IsAttckingCore = false;
            EntityType = EntityType.Enemy;
            timer = 0;
            _isAttacking  = false;
            _tempBasicMoveComponent.Init(MovementSpeed);//temp!
            
            var enemyConfig = (EnemyConfig)parameter;
            
            _decisionInterval = enemyConfig.DecisionInterval;
            _currentDecisionInterval = _decisionInterval;
            _aggroLevel = enemyConfig.AggroLevel;
            _returnLevel = enemyConfig.ReturnLevel;
        }

        protected override void UpdateEntity()
        {
            if (IsAttckingCore)
                Attack();
            
            if (_currentDecisionInterval < 0)
            {
                if (!_isAttacking)
                {
                    if (Random.Range(0, 100) < _aggroLevel)
                    {
                        if (TargetingHandler.HaveTarget)
                        {
                            _isAttacking  = true;
#if UNITY_EDITOR
                            Debug.Log($"{gameObject.name} InstanceID: {EntityInstanceID} is attacking {TargetingHandler.CurrentTarget.EntityTransform.name}");
#endif
                        }
                    }
                }

                if (_isAttacking)
                {
                    _tempBasicMoveComponent.SetDestination(TargetingHandler.CurrentTarget.EntityTransform.position, MoveType.Free);//temp!

                    if (Random.Range(0, 100) < _returnLevel +
                        Vector3.Distance(EntityTransform.position, _movementOnPath.CurrentPointOnPath) ||
                        TargetingHandler.CurrentTarget.IsEntityDead)
                    {
                        _isAttacking = false;
                        Debug.Log($"{gameObject.name} return to path");
                    }
                }
                else
                    _movementOnPath.AdvanceOnPath();

                _currentDecisionInterval = _decisionInterval;
            }
            
            if (_isAttacking)
            {
                if (!TargetingHandler.HaveTarget)
                {
                    _isAttacking = false;
                    return;
                }//plastr

                if (Vector3.Distance(transform.position, TargetingHandler.CurrentTarget.EntityTransform.position) < AttackRange.CurrentValue)
                    Attack();
            }
            
            _currentDecisionInterval -= GAME_TIME.GameDeltaTime;
        }

        public void TakeTarget(IEntityTargetAbleComponent target)
        {
            SetAttackTarget(target);
        }

        public override void Attack()
        {
            if (TargetingHandler.CurrentTarget == null)
                return;
            
            if (timer >= StatusHandler.GetStatById((int)Constant.Stats.AttackRate).CurrentValue)
            {
                timer = 0f;
                TargetingHandler.CurrentTarget.TakeDamage(StatusHandler.GetStatById((int)Constant.Stats.AttackDamage).CurrentValue, false);
                Debug.Log($"{gameObject.name} attack {TargetingHandler.CurrentTarget.EntityTransform.name}");
            }
            else
            {
                timer += GAME_TIME.GameDeltaTime;
                
            }
        }

        public override void OnEntityDead()
        {
            Dispose();
        }

        #region PoolObject

        public event Action<Enemy> OnDispose;
        
        public void Dispose()
        {
            OnDispose?.Invoke(this);
            gameObject.SetActive(false);
        }

        public void Free()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}