using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;

namespace Tzipory.Systems.TargetingSystem
{
    public abstract class BaseTargetingPriority : IPriorityTargeting
    {
        protected IEntityTargetingComponent TargetingComponent;

        protected BaseTargetingPriority(IEntityTargetingComponent targetingComponent)
        {
            TargetingComponent  = targetingComponent;
        }
        
        public abstract ITargetAbleEntity GetPriorityTarget(IEnumerable<ITargetAbleEntity> targets);
    }
}