using System.Collections.Generic;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using Tzipory.GameplayLogic.EntitySystem.TempleCore;
using UnityEngine;

namespace Tzipory.GameplayLogic.TargetingPriorities
{

    public class ClosestToCoreTarget : BaseTargetingPriority
    {
        public ClosestToCoreTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override IEntityTargetAbleComponent GetPriorityTarget(IEnumerable<IEntityTargetAbleComponent> targets)
        {
            IEntityTargetAbleComponent currentClosestTarget = null;

            float currentClosestDistance = float.MaxValue;

            foreach (var target in targets)
            {
                var distance = Vector3.Distance(CoreTemple.CoreTrans.position, target.EntityTransform.position);

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
