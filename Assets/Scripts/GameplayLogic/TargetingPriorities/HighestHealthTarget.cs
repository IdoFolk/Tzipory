using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;

namespace Tzipory.GameplayLogic.TargetingPriorities
{

    public class HighestHealthTarget : BaseTargetingPriority
    {
        public HighestHealthTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override ITargetAbleEntity GetPriorityTarget(IEnumerable<ITargetAbleEntity> targets)
        {
            ITargetAbleEntity currentHighestTarget = null;

            float currentLowestHP = float.MinValue;

            foreach (var target in targets)
            {
                if (target.EntityHealthComponent.Health.CurrentValue > currentLowestHP)
                {
                    currentHighestTarget = target;
                    currentLowestHP = target.EntityHealthComponent.Health.CurrentValue;
                }
            }

            return currentHighestTarget;
        }
    }
}
