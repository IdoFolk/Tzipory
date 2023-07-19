using System;
using Helpers.Consts;
using Sirenix.OdinInspector;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.EntitySystem.Entitys;
using Tzipory.Systems.PoolSystem;
using Tzipory.Tools.Interface;
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
        public BasicMoveComponent BasicMoveComponent;

        float timer;
        
        public override void Init(BaseUnitEntityConfig parameter)
        {
            base.Init(parameter);
            
            IsAttckingCore = false;
            EntityTeamType = EntityTeamType.Enemy;
            timer = 0;
            _isAttacking  = false;
            BasicMoveComponent.Init(MoveSpeed);//temp!
            
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
                        if (Targeting.HaveTarget)
                            _isAttacking  = true;
                    }
                }

                if (_isAttacking)
                {
                    BasicMoveComponent.SetDestination(Targeting.CurrentTarget.EntityTransform.position, MoveType.Free);//temp!

                    if (Random.Range(0, 100) < _returnLevel +
                        Vector3.Distance(EntityTransform.position, _movementOnPath.CurrentPointOnPath) ||
                        Targeting.CurrentTarget.IsEntityDead)
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
                if (!Targeting.HaveTarget)
                {
                    _isAttacking = false;
                    return;
                }//plastr

                if (Vector3.Distance(transform.position, Targeting.CurrentTarget.EntityTransform.position) < AttackRange.CurrentValue)
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
            if (Targeting.CurrentTarget == null)
                return;
            
            if (timer >= StatusHandler.GetStatById((int)Constant.Stats.AttackRate).CurrentValue)
            {
                timer = 0f;
                Targeting.CurrentTarget.TakeDamage(StatusHandler.GetStatById((int)Constant.Stats.AttackDamage).CurrentValue, false);
                Debug.Log($"{gameObject.name} attack {Targeting.CurrentTarget.EntityTransform.name}");
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