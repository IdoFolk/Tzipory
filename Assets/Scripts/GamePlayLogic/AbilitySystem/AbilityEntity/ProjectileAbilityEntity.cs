using Systems.TargetingSystem;
using Tools.Enums;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

namespace Tzipory.AbilitiesSystem.AbilityEntity
{
    public class ProjectileAbilityEntity : BaseAbilityEntity , ITargetableReciever
    {
        private float _penetrationNumber;
        private float _speed;
        private Vector3 _dir;
        
        public void Init(IEntityTargetAbleComponent target,float speed, float penetrationNumber,IAbilityExecutor abilityExecutor) 
        {
            base.Init(target, abilityExecutor);
            _speed = speed;
            _penetrationNumber = penetrationNumber;
            _dir = (target.EntityTransform.position - transform.position).normalized;
            visualTransform.up = _dir;
        }

        protected override void Update()
        {
            base.Update();
            
            transform.Translate(_dir * (_speed * GAME_TIME.GameDeltaTime));

            if (_penetrationNumber <= 0)
                Destroy(gameObject);
        }

        public void RecieveCollision(Collider2D other, IOStatType ioStatType)
        {
            
        }

        public void RecieveTargetableEntry(IEntityTargetAbleComponent targetable)
        {
            if (targetable.EntityInstanceID == _abilityExecutor.Caster.EntityInstanceID) return;
            
            _abilityExecutor.Init(targetable);
            _penetrationNumber--;
        }

        public void RecieveTargetableExit(IEntityTargetAbleComponent targetable)
        {
            
        }
    }
}