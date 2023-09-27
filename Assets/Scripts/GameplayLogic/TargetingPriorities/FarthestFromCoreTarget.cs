using System.Collections;
using System.Collections.Generic;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using UnityEngine;
namespace Tzipory.GameplayLogic.TargetingPriorities
{

    public class FarthestFromCoreTarget : BaseTargetingPriority
    {
        public FarthestFromCoreTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override IEntityTargetAbleComponent GetPriorityTarget(IEnumerable<IEntityTargetAbleComponent> targets)
        {
            IEntityTargetAbleComponent currentFarthestTarget = null;

            float currentLongestDistance = float.MinValue;

            foreach (var target in targets)
            {
                var distance = Vector3.Distance(CoreTemple.CoreTrans.position, target.EntityTransform.position);

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
