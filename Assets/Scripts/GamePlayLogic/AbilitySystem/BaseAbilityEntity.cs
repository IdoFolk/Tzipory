﻿using System;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Systems.AbilitySystem;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Systems.VisualSystem;
using UnityEngine;

namespace Tzipory.GamePlayLogic.AbilitySystem
{
    public abstract class BaseAbilityEntity : MonoBehaviour , IAbility ,ITargetableEntryReciever
    {
        public event Action OnCast;
        
        [SerializeField] protected AbilityVisualHandler _abilityVisualHandler;
        [SerializeField] private ColliderTargetingArea _colliderTargeting;
        
        protected IAbilityExecutor AbilityExecutor;
        protected ITargetAbleEntity Caster;
        
        
        public bool IsInitialization { get; private set; }
        
        
        public virtual void Init(ITargetAbleEntity caster ,Vector2 parameter,IAbilityExecutor executor,AbilityVisualConfig abilityVisualConfig)
        {
            Caster = caster;
            _colliderTargeting.Init(this);
            AbilityExecutor = executor;
        }
        
        public abstract void RecieveTargetableEntry(ITargetAbleEntity targetable);
    }
}