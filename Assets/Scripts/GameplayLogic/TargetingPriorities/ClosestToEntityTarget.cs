using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;

namespace Tzipory.GameplayLogic.TargetingPriorities
{
    public class ClosestToEntityTarget : BaseTargetingPriority
    {
        public ClosestToEntityTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override ITargetAbleEntity GetPriorityTarget(IEnumerable<ITargetAbleEntity> targets)
        {
            ITargetAbleEntity currentClosestTarget = null;
            
            float currentClosestTargetDistance = float.MaxValue;

            foreach (var target in targets)
            {
                var distance = TargetingComponent.GetDistanceToTarget(target);
                
                if (distance < currentClosestTargetDistance)
                {
                    currentClosestTarget = target;
                    currentClosestTargetDistance = distance;
                }
            }
            
            return currentClosestTarget;
        }
    }
}