using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;

namespace Tzipory.GameplayLogic.TargetingPriorities
{

    public class LowestHealthTarget : BaseTargetingPriority
    {
        public LowestHealthTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override IEntityTargetAbleComponent GetPriorityTarget(IEnumerable<IEntityTargetAbleComponent> targets)
        {
            IEntityTargetAbleComponent currentLowestTarget = null;

            float currentLowestHP = float.MaxValue;

            foreach (var target in targets)
            {
                if (target.Health.CurrentValue < currentLowestHP)
                {
                    currentLowestTarget = target;
                    currentLowestHP = target.Health.CurrentValue;
                }
            }

            return currentLowestTarget;
        }
    }
}
