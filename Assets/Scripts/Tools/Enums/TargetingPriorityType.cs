namespace Tzipory.EntitySystem.TargetingSystem
{
    public enum TargetingPriorityType
    {
        Default,
        Random,
        ClosestToEntity,
        FarthestFromEntity,
        ClosestToCore,
        FarthestFromCore,
        LowestHpEntity,
        HighestHpEntity,
        LowestHpPercentageEntity,
        HighestHpPercentageEntity,
    }
}