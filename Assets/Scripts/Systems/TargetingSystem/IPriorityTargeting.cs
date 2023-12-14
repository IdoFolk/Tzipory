using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;

namespace Tzipory.Systems.TargetingSystem
{
    public interface IPriorityTargeting
    {
        public ITargetAbleEntity GetPriorityTarget(IEnumerable<ITargetAbleEntity> targets);
    }
}