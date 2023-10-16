using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;

namespace Tzipory.GameplayLogic.TargetingPriorities
{

    public class HighestHealthPercentageTarget : BaseTargetingPriority
    {
        public HighestHealthPercentageTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override IEntityTargetAbleComponent GetPriorityTarget(IEnumerable<IEntityTargetAbleComponent> targets)
        {
            IEntityTargetAbleComponent currentLowestTarget = null;

            float currentHighestHP = float.MinValue;

            foreach (var target in targets)
            {
                //USE BASE VALUE HERE! NOT MAXVALUE! 
                if (target.Health.CurrentValue / target.Health.BaseValue > currentHighestHP)
                {
                    currentLowestTarget = target;
                    currentHighestHP = target.Health.CurrentValue;
                }
            }

            return currentLowestTarget;
        }
    }
}
