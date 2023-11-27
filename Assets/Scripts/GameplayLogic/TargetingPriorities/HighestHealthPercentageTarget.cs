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

        public override ITargetAbleEntity GetPriorityTarget(IEnumerable<ITargetAbleEntity> targets)
        {
            ITargetAbleEntity currentLowestTarget = null;

            float currentHighestHP = float.MinValue;

            foreach (var target in targets)
            {
                //USE BASE VALUE HERE! NOT MAXVALUE! 
                if (target.EntityHealthComponent.Health.CurrentValue / target.EntityHealthComponent.Health.BaseValue > currentHighestHP)
                {
                    currentLowestTarget = target;
                    currentHighestHP = target.EntityHealthComponent.Health.CurrentValue;
                }
            }

            return currentLowestTarget;
        }
    }
}
