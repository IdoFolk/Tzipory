using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.GamePlayLogic.AbilitySystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.AbilitySystem;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace GamePlayLogic.AbilitySystem.AbilityEntity
{
    public class ProjectileAbilityEntity : BaseAbilityEntity
    {
        private float _penetrationNumber;
        private float _speed;
        private Vector3 _dir;
        
        public override void Init(ITargetAbleEntity caster, Vector2 parameter,IAbilityExecutor executor,AbilityVisualConfig abilityVisualConfig)
        {
            base.Init(caster, parameter,executor,abilityVisualConfig);
            _speed = caster.EntityStatComponent.GetStat(Constant.StatsId.ProjectileSpeed).CurrentValue;
            _penetrationNumber = caster.EntityStatComponent.GetStat(Constant.StatsId.ProjectilePenetration).CurrentValue;

            Instantiate(abilityVisualConfig.VisualObject, _abilityVisualHandler.transform);
            
            _dir = (parameter - (Vector2)transform.position).normalized;
            transform.up = _dir;
        }

        private void Update()
        {
            transform.position += transform.up * (_speed * GAME_TIME.GameDeltaTime);
            
            if (_penetrationNumber <= 0)
                Destroy(gameObject);
        }

        public override void RecieveTargetableEntry(ITargetAbleEntity targetable)
        {
            if (targetable.EntityType == Caster.EntityType)
                return;
            
            AbilityExecutor.Execute(targetable);
            _penetrationNumber--;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + _dir * 5);
        }
    }
}