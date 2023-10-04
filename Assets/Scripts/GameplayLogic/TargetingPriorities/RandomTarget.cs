using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.TargetingPriorities
{
    public class RandomTarget : BaseTargetingPriority
    {
        public RandomTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override IEntityTargetAbleComponent GetPriorityTarget(IEnumerable<IEntityTargetAbleComponent> targets)
        {
            List<IEntityTargetAbleComponent> tempList = targets.ToList();
            if (tempList.Count == 0)
                return null;
            return tempList[Random.Range(0, tempList.Count- 1)];
        }
    }
}