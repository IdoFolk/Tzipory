using System.Collections;
using System.Collections.Generic;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;
namespace Tzipory.EntitySystem.TargetingSystem.TargetingPriorites
{

    public class FarthestFromEntityTarget : BaseTargetingPriority
    {
        public FarthestFromEntityTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override IEntityTargetAbleComponent GetPriorityTarget(IEnumerable<IEntityTargetAbleComponent> targets)
        {
            IEntityTargetAbleComponent currentFarthestTarget = null;

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
