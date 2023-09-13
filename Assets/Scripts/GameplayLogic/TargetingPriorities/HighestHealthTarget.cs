using System.Collections;
using System.Collections.Generic;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.TargetingSystem;
using UnityEngine;
namespace Tzipory.GameplayLogic.TargetingPriorities
{

    public class HighestHealthTarget : BaseTargetingPriority
    {
        public HighestHealthTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override IEntityTargetAbleComponent GetPriorityTarget(IEnumerable<IEntityTargetAbleComponent> targets)
        {
            IEntityTargetAbleComponent currentHighestTarget = null;

            float currentLowestHP = float.MinValue;

            foreach (var target in targets)
            {
                if (target.Health.CurrentValue > currentLowestHP)
                {
                    currentHighestTarget = target;
                    currentLowestHP = target.Health.CurrentValue;
                }
            }

            return currentHighestTarget;
        }
    }
}
