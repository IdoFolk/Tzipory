using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.GamePlayLogic.AbilitySystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.AbilitySystem;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace GamePlayLogic.AbilitySystem.AbilityEntity
{
    public class AOEAbility : BaseAbility , ITargetableExitReciever
    {
        private float _duration;
        
        protected override void Init(ITargetAbleEntity caster, Vector2 parameter, IAbilityExecutor executor, AbilityConfig config)
        {
            base.Init(caster, parameter, executor, config);
            _duration = caster.EntityStatComponent.GetStat(Constant.StatsId.AoeDuration).CurrentValue;
            
            //_visualTransform.localScale  = new Vector3(radius , radius, 1); //why *2.5?
        }
        
        private void Update()
        {
            _duration -= GAME_TIME.GameDeltaTime;//need to be a timer
        
            if(_duration <= 0)
                Destroy(gameObject);//TODO: add a pool to the ability entity system
        }

        public override void RecieveTargetableEntry(ITargetAbleEntity targetable)
        {
            AbilityExecutor.Execute(targetable);
        }

        public void RecieveTargetableExit(ITargetAbleEntity targetable)
        {
        }
    }
}