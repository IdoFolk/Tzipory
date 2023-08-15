using Tzipory.EntitySystem.StatusSystem;
using Tzipory.EntitySystem.TargetingSystem;
using UnityEngine;

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityTargetingComponent : IEntityComponent
    {
        //TODO may need to add OnTargetSelected event
        public Stat TargetingRange { get; }
        public EntityType EntityType { get; }
        public Vector2 ShotPosition { get;}
        public IPriorityTargeting DefaultPriorityTargeting { get; }
        public TargetingHandler TargetingHandler { get; }
        public float GetDistanceToTarget(IEntityTargetAbleComponent targetAbleComponent);
    }
}