using Tzipory.Systems.AbilitySystem.AbilityExecuteTypes;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Tools.Enums;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.Systems.AbilitySystem.AbilityEntity
{
    public class AoeAbilityEntity : BaseAbilityEntity, ITargetableReciever
    {
        [SerializeField] private ColliderTargetingArea _colliderTargetingArea;
        
        private float _duration;
        private AoeAbilityExecuter _aoeAbilityExecuter;
    
        public void Init(IEntityTargetAbleComponent target, float radius, float duration, AoeAbilityExecuter abilityExecutor)
        {
            base.Init(target,abilityExecutor);

            _renderer.playableAsset = abilityExecutor.Visual;
            
            _aoeAbilityExecuter = abilityExecutor;
            _duration = duration;
            _colliderTargetingArea.Init(this);
            _visualTransform.localScale  = new Vector3(radius , radius, 1); //why *2.5?
            
            _renderer.Play();
        }

        public void RecieveCollision(Collider2D other, IOType ioType)
        {
            
        }

        public void RecieveTargetableEntry(IEntityTargetAbleComponent targetable)
        {
            if (targetable == _aoeAbilityExecuter.Caster)
                return;
        
            _aoeAbilityExecuter.Execute(targetable);
        }

        public void RecieveTargetableExit(IEntityTargetAbleComponent targetable)
        {
            if (targetable == _aoeAbilityExecuter.Caster)
                return;
        
            _aoeAbilityExecuter.ExecuteOnExit(targetable);
        }

        protected override void Update()
        {
            base.Update();
        
            _duration -= GAME_TIME.GameDeltaTime;//need to be a timer
        
            if(_duration <= 0)
                Destroy(gameObject);//TODO: add a pool to the ability entity system
        }
    }
}
