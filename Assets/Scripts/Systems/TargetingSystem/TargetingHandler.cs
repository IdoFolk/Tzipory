using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.Tools.Enums;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.Systems.TargetingSystem
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
            
            transform.localScale = new Vector3(_entityTargetingComponent.TargetingRange.CurrentValue, _entityTargetingComponent.TargetingRange.CurrentValue,1f);
            
            _entityTargetingComponent.TargetingRange.OnValueChangedData += UpdateTargetingRange;
        }

        private void UpdateTargetingRange(StatChangeData statChangeData)=>
            transform.localScale = new Vector3(statChangeData.NewValue, statChangeData.NewValue,1f);

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
                CurrentTarget = _entityTargetingComponent.DefaultPriorityTargeting.GetPriorityTarget(_availableTargets);
            else
                CurrentTarget = priorityTargeting.GetPriorityTarget(_availableTargets);

            if (!CurrentTarget.GameEntity.gameObject.activeInHierarchy)
            {
                RemoveTarget(CurrentTarget);
                return GetPriorityTarget();
            }
            
            return true;
        }

        private void TryAddTarget(IEntityTargetAbleComponent targetAbleComponent)
        {
            if (targetAbleComponent.EntityType == _entityTargetingComponent.EntityType)
                return;
#if UNITY_EDITOR
            Debug.Log($"<color=#f2db05>Targeting Handler:</color> Entity: <color=#de05f2>{_entityTargetingComponent.GameEntity.name}</color>: added {targetAbleComponent.GameEntity.name} to targets list");
#endif
            if (!targetAbleComponent.IsTargetAble)
                return;
            
            targetAbleComponent.OnTargetDisable += RemoveTarget;
            _availableTargets.Add(targetAbleComponent);
        }

        private void RemoveTarget(IEntityTargetAbleComponent targetAbleComponent)
        {
            if (_availableTargets.Contains(targetAbleComponent))
            {
#if UNITY_EDITOR
                Debug.Log($"<color=#f2db05>Targeting Handler:</color> Entity: <color=#de05f2>{_entityTargetingComponent.GameEntity.name}</color>: Remove {targetAbleComponent.GameEntity.name} from targets list entity");
#endif
                
                targetAbleComponent.OnTargetDisable -= RemoveTarget;
                _availableTargets.Remove(targetAbleComponent);
                
                if (targetAbleComponent == CurrentTarget)
                    GetPriorityTarget();
            }
        }


        public void RecieveCollision(Collider2D other, IOType ioType)
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

        public void Reset()
        {
            _availableTargets.Clear();
            _entityTargetingComponent.TargetingRange.OnValueChangedData -= UpdateTargetingRange;
        }
    }
}