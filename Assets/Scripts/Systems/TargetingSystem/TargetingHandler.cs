using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Enums;
using Tzipory.Tools.Interface;
using UnityEngine;
using Logger = Tzipory.Tools.Debag.Logger;

namespace Tzipory.Systems.TargetingSystem
{
    public class TargetingHandler : MonoBehaviour , ITargetableAllReciever
    {
        private const string TARGETING_HANDLER_LOG_GROUP = "TargetingHandler";
        
        [SerializeField,Required] private ColliderTargetingArea _targetingArea;

        private IEntityTargetingComponent _entityTargetingComponent;
        private List<IEntityTargetAbleComponent> _availableTargets;
        
        public bool HaveTarget => CurrentTarget != null;
        public IEntityTargetAbleComponent CurrentTarget { get; private set; }
        
        public List<IEntityTargetAbleComponent> AvailableTargets => _availableTargets;

        public void Init(IEntityTargetingComponent targetingComponent)
        {
            _availableTargets = new List<IEntityTargetAbleComponent>();
            _entityTargetingComponent = targetingComponent;
            
            _targetingArea.Init(this);
            
            transform.localScale = new Vector3(_entityTargetingComponent.TargetingRange.CurrentValue, _entityTargetingComponent.TargetingRange.CurrentValue,1f);
            
            _entityTargetingComponent.TargetingRange.OnValueChanged += UpdateTargetingRange;
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
            
            Logger.Log($"Entity: <color=#de05f2>{_entityTargetingComponent.GameEntity.name}</color>: added {targetAbleComponent.GameEntity.name} to targets list",TARGETING_HANDLER_LOG_GROUP);
            
            if (!targetAbleComponent.IsTargetAble)
                return;
            
            targetAbleComponent.OnTargetDisable += RemoveTarget;
            _availableTargets.Add(targetAbleComponent);
        }

        private void RemoveTarget(IEntityTargetAbleComponent targetAbleComponent)
        {
            if (_availableTargets.Contains(targetAbleComponent))
            {
                Logger.Log($"Entity: <color=#de05f2>{_entityTargetingComponent.GameEntity.name}</color>: Remove {targetAbleComponent.GameEntity.name} from targets list entity",TARGETING_HANDLER_LOG_GROUP);
                
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
            _entityTargetingComponent.TargetingRange.OnValueChanged -= UpdateTargetingRange;
        }
    }
}