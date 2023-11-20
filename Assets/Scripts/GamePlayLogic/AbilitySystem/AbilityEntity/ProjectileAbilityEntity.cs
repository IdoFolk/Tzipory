using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Tools.Enums;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.Systems.AbilitySystem.AbilityEntity
{
    public class ProjectileAbilityEntity : BaseAbilityEntity , ITargetableReciever
    {
        [SerializeField] private ColliderTargetingArea _colliderTargeting;
        
        private float _penetrationNumber;
        private float _speed;
        private Vector3 _dir;
        
        public void Init(IEntityTargetAbleComponent target,float speed, float penetrationNumber,IAbilityExecutor abilityExecutor) 
        {
            base.Init(target, abilityExecutor);
            _colliderTargeting.Init(this);
            _speed = speed;
            _penetrationNumber = penetrationNumber;
            _dir = (target.EntityTransform.position - transform.position).normalized;
            _visualTransform.up = _dir;
        }

        protected override void Update()
        {
            base.Update();
            
            transform.Translate(_dir * (_speed * GAME_TIME.GameDeltaTime));

            if (_penetrationNumber <= 0)
                Destroy(gameObject);
        }

        public void RecieveCollision(Collider2D other, IOType ioType)
        {
            
        }

        public void RecieveTargetableEntry(IEntityTargetAbleComponent targetable)
        {
            if (targetable.EntityInstanceID == AbilityExecutor.Caster.EntityInstanceID) return;
            
            // if (targetable.EntityType == Caster.EntityType)
            //     return;

            AbilityExecutor.Init(targetable);
            _penetrationNumber--;
        }

        public void RecieveTargetableExit(IEntityTargetAbleComponent targetable)
        {
            
        }
    }
}