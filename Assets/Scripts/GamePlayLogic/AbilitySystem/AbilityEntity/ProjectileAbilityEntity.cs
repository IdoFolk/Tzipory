using System.Collections.Generic;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.Systems.AbilitySystem
{
    public class ProjectileAbilityEntity : BaseAbilityEntity , ITargetableEntryReciever
    {
        [SerializeField] private ColliderTargetingArea _colliderTargeting;
        
        private float _penetrationNumber;
        private float _speed;
        private Vector3 _dir;

        protected override void Init(ITargetAbleEntity target,IAbilityExecutor parameter1, AbilityConfig parameter2, Dictionary<int, Stat> stats)
        {
           // base.Init(parameter1, parameter2, stats);

            float speed = 0;
            float penetrationNumber = 1;

            if (stats.TryGetValue((int)Constant.StatsId.ProjectileSpeed, out var speedStat))
            {
                speed = speedStat.CurrentValue;
            }

            if (stats.TryGetValue((int)Constant.StatsId.ProjectilePenetration, out var penetrationNumberStat))
            {
                penetrationNumber = penetrationNumberStat.CurrentValue;
            }
            
            _colliderTargeting.Init(this);
            _speed = speed;
            _penetrationNumber = penetrationNumber;
            _dir = (target.GameEntity.transform.position - transform.position).normalized;
            transform.up = _dir;
        }

        public void Init(ITargetAbleEntity target,float speed, float penetrationNumber,IAbilityExecutor abilityExecutor) 
        {
         //   base.Init(abilityExecutor);
            
            
        }

        protected override void Update()
        {
            base.Update();
            
            transform.Translate(_dir * (_speed * GAME_TIME.GameDeltaTime));

            if (_penetrationNumber <= 0)
                Destroy(gameObject);
        }

        public void RecieveTargetableEntry(ITargetAbleEntity targetable)
        {
           // if (targetable.GameEntity.EntityInstanceID == AbilityExecutor.Caster.GameEntity.EntityInstanceID) return;
            
            //AbilityExecutor.Execute(targetable);
            _penetrationNumber--;
        }
    }
}