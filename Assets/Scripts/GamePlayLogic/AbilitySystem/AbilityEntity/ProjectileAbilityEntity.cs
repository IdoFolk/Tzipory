using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

namespace Tzipory.AbilitiesSystem.AbilityEntity
{
    public class ProjectileAbilityEntity : BaseAbilityEntity
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<IEntityTargetAbleComponent>(out var targetAbleComponent)) return;
            
            if (targetAbleComponent.EntityInstanceID == _abilityExecutor.Caster.EntityInstanceID) return;
            
            _abilityExecutor.Init(targetAbleComponent);
            _penetrationNumber--;
        }
    }
}