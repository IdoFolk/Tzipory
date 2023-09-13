using System;
using Tzipory.ConfigFiles.PartyConfig.AbilitySystemConfig;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.TargetingSystem;
using Tzipory.GameplayLogic.TargetingPriorities;

namespace Tzipory.Factory
{
    public class TargetingPriorityFactory
    {
        public static IPriorityTargeting GetTargetingPriority(IEntityTargetingComponent entityTargetingComponent,TargetingPriorityType targetingPriorityType)
        {
            switch (targetingPriorityType)
            {
                case TargetingPriorityType.Random:
                    return new RandomTarget(entityTargetingComponent);
                case TargetingPriorityType.ClosestToEntity:
                    return new ClosestToEntityTarget(entityTargetingComponent);
                case TargetingPriorityType.FarthestFromEntity:
                    return new FarthestFromEntityTarget(entityTargetingComponent);
                case TargetingPriorityType.ClosestToCore:
                    return new ClosestToCoreTarget(entityTargetingComponent);
                case TargetingPriorityType.FarthestFromCore:
                    return new FarthestFromCoreTarget(entityTargetingComponent);
                case TargetingPriorityType.LowestHpEntity:
                    return new LowestHealthTarget(entityTargetingComponent);
                case TargetingPriorityType.HighestHpEntity:
                    return new HighestHealthTarget(entityTargetingComponent);
                case TargetingPriorityType.LowestHpPercentageEntity:
                    return new LowestHealthPercentageTarget(entityTargetingComponent);
                case TargetingPriorityType.HighestHpPercentageEntity:
                    return new HighestHealthPercentageTarget(entityTargetingComponent);
                case TargetingPriorityType.Default:
                    return entityTargetingComponent.DefaultPriorityTargeting ?? new ClosestToEntityTarget(entityTargetingComponent);
                default:
                    throw  new ArgumentOutOfRangeException();
            }
        }
    }
}