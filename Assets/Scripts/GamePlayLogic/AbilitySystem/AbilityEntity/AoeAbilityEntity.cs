using Systems.TargetingSystem;
using Tools.Enums;
using Tzipory.AbilitiesSystem.AbilityEntity;
using Tzipory.AbilitiesSystem.AbilityExecuteTypes;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

namespace GamePlayLogic.AbilitySystem.AbilityEntity
{
    public class AoeAbilityEntity : BaseAbilityEntity, ITargetableReciever
    {
        [SerializeField] private ColliderTargetingArea _colliderTargetingArea;
        
        private float _duration;
        private AoeAbilityExecuter _aoeAbilityExecuter;
    
        public void Init(IEntityTargetAbleComponent target, float radius, float duration, AoeAbilityExecuter abilityExecutor)
        {
            base.Init(target,abilityExecutor);
            _aoeAbilityExecuter = abilityExecutor;
            _duration = duration;
            _colliderTargetingArea.Init(this);
            visualTransform.localScale  = new Vector3(radius , radius, 1); //why *2.5?
        }

        public void RecieveCollision(Collider2D other, IOStatType ioStatType)
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
