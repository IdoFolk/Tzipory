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

        public override ITargetAbleEntity GetPriorityTarget(IEnumerable<ITargetAbleEntity> targets)
        {
            ITargetAbleEntity currentLowestTarget = null;

            float currentLowestHP = float.MaxValue;

            foreach (var target in targets)
            {
                if (target.EntityHealthComponent.Health.CurrentValue < currentLowestHP)
                {
                    currentLowestTarget = target;
                    currentLowestHP = target.EntityHealthComponent.Health.CurrentValue;
                }
            }

            return currentLowestTarget;
        }
    }
}
