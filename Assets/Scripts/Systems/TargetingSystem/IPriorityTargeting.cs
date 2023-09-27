using System.Collections.Generic;
using Tzipory.EntitySystem.EntityComponents;

namespace Tzipory.Systems.TargetingSystem
{
    public interface IPriorityTargeting
    {
        public IEntityTargetAbleComponent GetPriorityTarget(IEnumerable<IEntityTargetAbleComponent> targets);
    }
}