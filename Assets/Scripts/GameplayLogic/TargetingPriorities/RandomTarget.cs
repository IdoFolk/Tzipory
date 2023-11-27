using System.Collections.Generic;
using System.Linq;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.TargetingPriorities
{
    public class RandomTarget : BaseTargetingPriority
    {
        public RandomTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override ITargetAbleEntity GetPriorityTarget(IEnumerable<ITargetAbleEntity> targets)
        {
            List<ITargetAbleEntity> tempList = targets.ToList();
            if (tempList.Count == 0)
                return null;
            return tempList[Random.Range(0, tempList.Count- 1)];
        }
    }
}