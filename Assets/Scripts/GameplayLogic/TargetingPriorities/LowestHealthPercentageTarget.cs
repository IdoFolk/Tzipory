using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;

namespace Tzipory.GameplayLogic.TargetingPriorities
{

    public class LowestHealthPercentageTarget : BaseTargetingPriority
    {
        public LowestHealthPercentageTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override IEntityTargetAbleComponent GetPriorityTarget(IEnumerable<IEntityTargetAbleComponent> targets)
        {
            IEntityTargetAbleComponent currentLowestTarget = null;

            float currentLowestHP = float.MaxValue;

            foreach (var target in targets)
            {
                //USE BASE VALUE HERE! NOT MAXVALUE! 
                if (target.Health.CurrentValue/target.Health.BaseValue < currentLowestHP)
                {
                    currentLowestTarget = target;
                    currentLowestHP = target.Health.CurrentValue;
                }
            }

            return currentLowestTarget;
        }
    }
}
