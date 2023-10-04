using System.Collections;
using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using UnityEngine;
namespace Tzipory.GameplayLogic.TargetingPriorities
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
