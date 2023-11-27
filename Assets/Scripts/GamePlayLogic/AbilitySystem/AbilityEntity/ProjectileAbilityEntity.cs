using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Tools.Interface;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.Systems.AbilitySystem.AbilityEntity
{
    public class ProjectileAbilityEntity : BaseAbilityEntity , ITargetableEntryReciever
    {
        [SerializeField] private ColliderTargetingArea _colliderTargeting;
        
        private float _penetrationNumber;
        private float _speed;
        private Vector3 _dir;
        
        public void Init(ITargetAbleEntity target,float speed, float penetrationNumber,IAbilityExecutor abilityExecutor) 
        {
            base.Init(abilityExecutor);
            
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

        public void RecieveTargetableEntry(ITargetAbleEntity targetable)
        {
            if (targetable.EntityInstanceID == AbilityExecutor.Caster.EntityInstanceID) return;
            
            AbilityExecutor.Execute(targetable);
            _penetrationNumber--;
        }
    }
}