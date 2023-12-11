using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Systems.AbilitySystem;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Systems.VisualSystem;
using UnityEngine;

namespace Tzipory.GamePlayLogic.AbilitySystem
{
    public abstract class BaseAbility : MonoBehaviour , IAbility ,ITargetableEntryReciever
    {
        [SerializeField] private AbilityVisualHandler _abilityVisualHandler;
        [SerializeField] private ColliderTargetingArea _colliderTargeting;
        
        protected IAbilityExecutor AbilityExecutor;
        protected ITargetAbleEntity Caster;
        
        public event Action OnCast;
        
        public Dictionary<int, Stat> Stats { get; private set; }
        
        public bool IsInitialization { get; private set; }
        
        
        protected virtual void Init(ITargetAbleEntity caster ,Vector2 parameter,IAbilityExecutor executor,AbilityConfig config)
        {
            Caster = caster;
            _colliderTargeting.Init(this);
        }
        
        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            IEnumerable<IStatHolder> statHolders = new List<IStatHolder>() { this };
            return statHolders;
        }
        public abstract void RecieveTargetableEntry(ITargetAbleEntity targetable);
    }
}