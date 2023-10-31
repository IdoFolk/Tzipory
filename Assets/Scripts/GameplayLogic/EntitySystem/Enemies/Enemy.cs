using System;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.EntityComponents;
using Tzipory.Systems.MovementSystem;
using Tzipory.Systems.PoolSystem;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tzipory.GameplayLogic.EntitySystem.Enemies
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
            _tempBasicMoveComponent.Init(StatHandler.GetStat(Constant.StatsId.MovementSpeed));//temp!
            
            var enemyConfig = (EnemyConfig)parameter;
            
            _decisionInterval = enemyConfig.DecisionInterval;
            _currentDecisionInterval = _decisionInterval;
            _aggroLevel = enemyConfig.AggroLevel;
            _returnLevel = enemyConfig.ReturnLevel;
            
            _movementOnPath.AdvanceOnPath();
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
            
            if (timer >= StatHandler.GetStat(Constant.StatsId.AttackRate).CurrentValue)
            {
                timer = 0f;
                float attackDamage = 0;

                attackDamage = TargetingHandler.CurrentTarget.EntityType == EntityType.Core 
                    ? StatHandler.GetStat(Constant.StatsId.CoreAttackDamage).CurrentValue 
                    : StatHandler.GetStat(Constant.StatsId.AttackDamage).CurrentValue;
                
                TargetingHandler.CurrentTarget.TakeDamage(attackDamage, false);
            }
            else
            {
                timer += GAME_TIME.GameDeltaTime;
            }
        }

        public override void StartDeathSequence()
        {
            base.StartDeathSequence();
            _tempBasicMoveComponent.Stop();
        }

        protected override void EntityDied()
        {
            base.EntityDied();
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