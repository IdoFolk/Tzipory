using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;

namespace Tzipory.Systems.TargetingSystem
{
    public interface ITargeting
    {
        public List<IEntityTargetAbleComponent> AvailableTargets { get; }
        
        public void GetPriorityTarget(IPriorityTargeting priorityTargeting = null);

        public void AddTarget(IEntityTargetAbleComponent targetAbleComponent);
        public void RemoveTarget(IEntityTargetAbleComponent targetAbleComponent);
    }
}