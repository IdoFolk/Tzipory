using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.TargetingPriorities
{

    public class FarthestFromCoreTarget : BaseTargetingPriority
    {
        public FarthestFromCoreTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override ITargetAbleEntity GetPriorityTarget(IEnumerable<ITargetAbleEntity> targets)
        {
            ITargetAbleEntity currentFarthestTarget = null;

            float currentLongestDistance = float.MinValue;

            foreach (var target in targets)
            {
                var distance = Vector3.Distance(CoreTemple.CoreTrans.position, target.GameEntity.transform.position);

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
