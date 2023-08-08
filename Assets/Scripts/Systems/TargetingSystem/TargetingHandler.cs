using System.Collections.Generic;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

namespace Tzipory.EntitySystem.TargetingSystem
{
    public class TargetingHandler : MonoBehaviour
    {
        [SerializeField] private Collider2D _targetingCollider2D;
        
        private IEntityTargetingComponent _entityTargetingComponent;
        private List<IEntityTargetAbleComponent> _availableTargets;
        
        public bool HaveTarget => CurrentTarget != null;
        public IEntityTargetAbleComponent CurrentTarget { get; private set; }
        
        public List<IEntityTargetAbleComponent> AvailableTargets => _availableTargets;

        public TargetingHandler(IEntityTargetingComponent targetingComponent)//may whnt to not be a monobehavior
        {
            _availableTargets = new List<IEntityTargetAbleComponent>();
            _entityTargetingComponent = targetingComponent;
        }

        public void Init(IEntityTargetingComponent targetingComponent)
        {
            _availableTargets = new List<IEntityTargetAbleComponent>();
            _entityTargetingComponent = targetingComponent;
            
            _targetingCollider2D.isTrigger  = true;
            _targetingCollider2D.transform.localScale = new Vector3(_entityTargetingComponent.TargetingRange.CurrentValue* 1.455f, _entityTargetingComponent.TargetingRange.CurrentValue,1f);//temp need to be const
            
            _entityTargetingComponent.TargetingRange.OnValueChanged += UpdateTargetingRange;
        }

        private void UpdateTargetingRange(float value)
        {
            _targetingCollider2D.transform.localScale = new Vector3(value * 1.455f, value,1f);//temp
        }

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

        private void AddTarget(IEntityTargetAbleComponent targetAbleComponent)
        {
            _availableTargets.Add(targetAbleComponent);
        }

        private void RemoveTarget(IEntityTargetAbleComponent targetAbleComponent)
        {
            _availableTargets.Remove(targetAbleComponent);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IEntityTargetAbleComponent targetAbleComponent) && targetAbleComponent.EntityTeamType != _entityTargetingComponent.EntityTeamType) //Removing friendly fire!
                AddTarget(targetAbleComponent);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IEntityTargetAbleComponent targetAbleComponent))
            {
                RemoveTarget(targetAbleComponent);
                
                if (CurrentTarget == null)
                    return;
                
                if (targetAbleComponent.EntityInstanceID == CurrentTarget.EntityInstanceID) //TBD targeting could be improved here
                    GetPriorityTarget();
            }
        }
    }
}