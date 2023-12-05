using System.Collections.Generic;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Systems.VisualSystem;
using UnityEngine;

namespace Tzipory.Systems.AbilitySystem
{
    public abstract class BaseAbilityEntity : BaseGameEntity , ITargetableEntryReciever,ITargetableExitReciever
    {
        [SerializeField] private ColliderTargetingArea _colliderTargetingArea;
        [SerializeField] private Transform _range;
        [SerializeField] protected AbilityVisualHandler _abilityVisualHandler;
        
        private IAbilityExecutor _abilityExecutor;
        
        public bool IsInitialization { get; private set; }

        protected virtual void Init(ITargetAbleEntity target,IAbilityExecutor parameter1, AbilityConfig parameter2,Dictionary<int,Stat> stats)
        {
            _abilityExecutor = parameter1;
            _colliderTargetingArea.Init(this);
            IsInitialization = true;
        }

        public void RecieveTargetableEntry(ITargetAbleEntity targetable)
        {
            _abilityExecutor.Execute(targetable);
        }

        public void RecieveTargetableExit(ITargetAbleEntity targetable)
        {
            
        }
    }
}