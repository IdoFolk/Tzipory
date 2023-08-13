using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Systems.TargetingSystem;
using Tools.Enums;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

namespace Tzipory.EntitySystem.TargetingSystem
{
    public class TargetingHandler : MonoBehaviour , ITargetableReciever
    {
        [SerializeField,Required] private ColliderTargetingArea _targetingArea;

        private IEntityTargetingComponent _entityTargetingComponent;
        private List<IEntityTargetAbleComponent> _availableTargets;
        
        public bool HaveTarget => CurrentTarget != null;
        public IEntityTargetAbleComponent CurrentTarget { get; private set; }
        
        public List<IEntityTargetAbleComponent> AvailableTargets => _availableTargets;
        
        [Obsolete]
        public TargetingHandler(IEntityTargetingComponent targetingComponent)//may whnt to not be a monobehavior
        {
            _availableTargets = new List<IEntityTargetAbleComponent>();
            _entityTargetingComponent = targetingComponent;
        }

        public void Init(IEntityTargetingComponent targetingComponent)
        {
            _availableTargets = new List<IEntityTargetAbleComponent>();
            _entityTargetingComponent = targetingComponent;
            
            _targetingArea.Init(this);
            
            UpdateTargetingRange(_entityTargetingComponent.TargetingRange.CurrentValue);
            
            _entityTargetingComponent.TargetingRange.OnValueChanged += UpdateTargetingRange;
        }

        private void UpdateTargetingRange(float value)=>
            transform.localScale = new Vector3(value, value,1f);

        public void SetAttackTarget(IEntityTargetAbleComponent target)
        {
            CurrentTarget = target;
        }

        public bool GetPriorityTarget(IPriorityTargeting priorityTargeting = null)
        {
            if (CurrentTarget is { IsEntityDead: true })
                CurrentTarget = null;

            if (_availableTargets.Count == 0)
                return false;

            if (priorityTargeting == null)
            {
                CurrentTarget = _entityTargetingComponent.DefaultPriorityTargeting.GetPriorityTarget(_availableTargets);
                return true;
            }
            
            CurrentTarget = priorityTargeting.GetPriorityTarget(_availableTargets);
            return true;
        }

        private void TryAddTarget(IEntityTargetAbleComponent targetAbleComponent)
        {
            if (targetAbleComponent.EntityType == _entityTargetingComponent.EntityType)
                return;
            
            _availableTargets.Add(targetAbleComponent);
        }

        private void RemoveTarget(IEntityTargetAbleComponent targetAbleComponent)
        {
            if (_availableTargets.Contains(targetAbleComponent))
                _availableTargets.Remove(targetAbleComponent);
        }


        public void RecieveCollision(Collider2D other, IOStatType ioStatType)
        {
            
        }

        public void RecieveTargetableEntry(IEntityTargetAbleComponent targetable)
        {
            TryAddTarget(targetable);
        }

        public void RecieveTargetableExit(IEntityTargetAbleComponent targetable)
        {
            RemoveTarget(targetable);

            if (CurrentTarget == null) return;
            if (targetable.EntityInstanceID == CurrentTarget.EntityInstanceID)
                GetPriorityTarget();
        }
    }
}