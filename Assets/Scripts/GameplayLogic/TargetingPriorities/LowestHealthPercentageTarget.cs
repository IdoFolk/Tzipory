using System.Collections.Generic;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;

namespace Tzipory.GameplayLogic.TargetingPriorities
{

    public class LowestHealthPercentageTarget : BaseTargetingPriority
    {
        public LowestHealthPercentageTarget(IEntityTargetingComponent targetingComponent) : base(targetingComponent)
        {
        }

        public override ITargetAbleEntity GetPriorityTarget(IEnumerable<ITargetAbleEntity> targets)
        {
            ITargetAbleEntity currentLowestTarget = null;

            float currentLowestHP = float.MaxValue;

            foreach (var target in targets)
            {
                //USE BASE VALUE HERE! NOT MAXVALUE! 
                if (target.EntityHealthComponent.Health.CurrentValue/target.EntityHealthComponent.Health.BaseValue < currentLowestHP)
                {
                    currentLowestTarget = target;
                    currentLowestHP = target.EntityHealthComponent.Health.CurrentValue;
                }
            }

            return currentLowestTarget;
        }
    }
}
