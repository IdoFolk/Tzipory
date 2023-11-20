using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.TargetingSystem;
using UnityEngine;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityTargetingComponent : IEntityComponent
    {
        //TODO may need to add OnTargetSelected event
        public Stat TargetingRange { get; }
        public EntityType EntityType { get; }
        public Vector2 ShotPosition { get;}
        public IPriorityTargeting DefaultPriorityTargeting { get; }
        public float GetDistanceToTarget(IEntityTargetAbleComponent targetAbleComponent);
    }
}