using System.Collections;
using System.Collections.Generic;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;
namespace Tzipory.EntitySystem.TargetingSystem.TargetingPriorites
{

    public class LowestHealthTarget : BaseTargetingPriority
    {
        public LowestHealthTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override IEntityTargetAbleComponent GetPriorityTarget(IEnumerable<IEntityTargetAbleComponent> targets)
        {
            IEntityTargetAbleComponent currentLowestTarget = null;

            float currentLowestHP = float.MaxValue;

            foreach (var target in targets)
            {
                if (target.HP.CurrentValue < currentLowestHP)
                {
                    currentLowestTarget = target;
                    currentLowestHP = target.HP.CurrentValue;
                }
            }

            return currentLowestTarget;
        }
    }
}
