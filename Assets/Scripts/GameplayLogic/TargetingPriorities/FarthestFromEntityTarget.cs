using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;

namespace Tzipory.GameplayLogic.TargetingPriorities
{

    public class FarthestFromEntityTarget : BaseTargetingPriority
    {
        public FarthestFromEntityTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override ITargetAbleEntity GetPriorityTarget(IEnumerable<ITargetAbleEntity> targets)
        {
            ITargetAbleEntity currentFarthestTarget = null;

            float currentLongestDistance = float.MinValue;

            foreach (var target in targets)
            {
                var distance = TargetingComponent.GetDistanceToTarget(target);

                if (distance > currentLongestDistance)
                {
                    currentFarthestTarget = target;
                    currentLongestDistance = distance;
                }
            }

            return currentFarthestTarget;
        }
    }
}
