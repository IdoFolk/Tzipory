using Tzipory.EntitySystem.StatusSystem;
using Tzipory.EntitySystem.TargetingSystem;

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityTargetingComponent : IEntityComponent
    {
        public Stat TargetingRange { get; }
        public EntityType EntityType { get; }
        public IPriorityTargeting DefaultPriorityTargeting { get; }
        public TargetingHandler TargetingHandler { get; }
        public float GetDistanceToTarget(IEntityTargetAbleComponent targetAbleComponent);
    }
}