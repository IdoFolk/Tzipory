using System;
using Helpers.Consts;
using Sirenix.OdinInspector;
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
        [SerializeField,TabGroup("AI")] private float _decisionInterval;//temp
        [SerializeField,TabGroup("AI")] private float _aggroLevel;//temp
        [SerializeField,TabGroup("AI")] private float _returnLevel;//temp

        private bool _isAttacking;

        private float _currentDecisionInterval;
        
        //TEMP!
        [SerializeField] private MovementOnPath _movementOnPath;
        public BasicMoveComponent BasicMoveComponent;

        float timer;
        
        public override void Init(BaseUnitEntityConfig parameter)
        {
            base.Init(parameter);
            EntityTeamType = EntityTeamType.Enemy;
            timer = 0;
            _currentDecisionInterval = _decisionInterval;
            _isAttacking  = false;
            BasicMoveComponent.Init(MoveSpeed);//temp!
        }

        protected override void UpdateEntity()
        {
            if (_currentDecisionInterval < 0)
            {
                if (!_isAttacking)
                {
                    if (Random.Range(0, 100) < _aggroLevel)
                    {
                        Targeting.GetPriorityTarget();
                    
                        if (!Targeting.HaveTarget)
                            return;
                        _isAttacking  = true;
                    }
                }
                
                if (_isAttacking)
                {
                    if (_returnLevel + Vector3.Distance(EntityTransform.position,_movementOnPath.CurrentPointOnPath) < Random.Range(0,100) || Targeting.CurrentTarget.IsEntityDead)
                        _isAttacking = false;
                }

                _currentDecisionInterval = _decisionInterval;
                _movementOnPath.AdvanceOnPath();
            }

            _currentDecisionInterval -= GAME_TIME.GameDeltaTime;

            if (_isAttacking)
            {
                BasicMoveComponent.SetDestination(Targeting.CurrentTarget.EntityTransform.position, MoveType.Free);//temp!
                
                if (Vector3.Distance(transform.position, Targeting.CurrentTarget.EntityTransform.position) < AttackRange.CurrentValue)
                    Attack();
            }
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