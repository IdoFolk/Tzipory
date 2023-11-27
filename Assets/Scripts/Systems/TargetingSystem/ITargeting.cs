using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;

namespace Tzipory.Systems.TargetingSystem
{
    public interface ITargeting
    {
        public List<ITargetAbleEntity> AvailableTargets { get; }
        
        public void GetPriorityTarget(IPriorityTargeting priorityTargeting = null);

        public void AddTarget(ITargetAbleEntity targetAbleEntity);
        public void RemoveTarget(ITargetAbleEntity targetAbleEntity);
    }
}