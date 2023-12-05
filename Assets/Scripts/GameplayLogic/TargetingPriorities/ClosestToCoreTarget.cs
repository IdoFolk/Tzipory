using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.TargetingPriorities
{

    public class ClosestToCoreTarget : BaseTargetingPriority
    {
        public ClosestToCoreTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override ITargetAbleEntity GetPriorityTarget(IEnumerable<ITargetAbleEntity> targets)
        {
            ITargetAbleEntity currentClosestTarget = null;

            float currentClosestDistance = float.MaxValue;

            foreach (var target in targets)
            {
                var distance = Vector3.Distance(CoreTemple.CoreTransform.position, target.GameEntity.transform.position);

                if (distance < currentClosestDistance)
                {
                    currentClosestTarget = target;
                    currentClosestDistance = distance;
                }
            }

            return currentClosestTarget;
        }
    }
}
