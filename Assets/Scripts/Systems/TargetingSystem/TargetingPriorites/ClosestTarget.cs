﻿using System.Collections.Generic;
using Tzipory.EntitySystem.EntityComponents;

namespace Tzipory.EntitySystem.TargetingSystem.TargetingPriorites
{
    public class ClosestTarget : BaseTargetingPriority
    {
        public ClosestTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override IEntityTargetAbleComponent GetPriorityTarget(IEnumerable<IEntityTargetAbleComponent> targets)
        {
            IEntityTargetAbleComponent currentClosestTarget = null;
            
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