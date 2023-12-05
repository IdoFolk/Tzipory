using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Systems.VisualSystem;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.GamePlayLogic.AbilitySystem
{
    public abstract class BaseAbility : MonoBehaviour , IAbility , ITargetableEntryReciever,ITargetableExitReciever 
    {
        [SerializeField] private AbilityVisualHandler _abilityVisualHandler;

        private ITargetAbleEntity _caster;
        
        public event Action OnCast;
        
        public Dictionary<int, Stat> Stats { get; private set; }
        
        public bool IsInitialization { get; private set; }
        
        
        protected virtual void Init(ITargetAbleEntity caster ,Vector2 parameter,AbilityConfig config)
        {
            _caster = caster;
        }
        
        public IEnumerable<IStatHolder> GetNestedStatHolders()
        {
            IEnumerable<IStatHolder> statHolders = new List<IStatHolder>() { this };
            return statHolders;
        }
        
        protected abstract void Execute(ITargetAbleEntity target);
        public abstract void RecieveTargetableEntry(ITargetAbleEntity targetable);
        public abstract void RecieveTargetableExit(ITargetAbleEntity targetable);
    }
}