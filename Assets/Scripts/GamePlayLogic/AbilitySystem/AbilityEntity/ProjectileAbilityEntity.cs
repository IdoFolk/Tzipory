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
        
        public override void Init(ITargetAbleEntity caster, Vector2 parameter,IAbilityExecutor executor)
        {
            base.Init(caster, parameter,executor);
            _speed = caster.EntityStatComponent.GetStat(Constant.StatsId.ProjectileSpeed).CurrentValue;
            _penetrationNumber = caster.EntityStatComponent.GetStat(Constant.StatsId.ProjectilePenetration).CurrentValue;
            
            _dir = parameter - (Vector2)transform.position.normalized;
            transform.up = _dir;
        }

        private void Update()
        {
            transform.Translate(_dir * (_speed * GAME_TIME.GameDeltaTime));

            if (_penetrationNumber <= 0)
                Destroy(gameObject);
        }

        public override void RecieveTargetableEntry(ITargetAbleEntity targetable)
        {
            AbilityExecutor.Execute(targetable);
            _penetrationNumber--;
        }
    }
}