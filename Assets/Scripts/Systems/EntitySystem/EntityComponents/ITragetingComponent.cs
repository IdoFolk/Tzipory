using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Tools.Interface;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityTargetingComponent : IEntityComponent , IStatHolder, IInitialization<BaseGameEntity,IInitialization<ITargetableReciever>,TargetingComponentConfig>
    {
        //TODO may need to add OnTargetSelected event
        
        public Stat TargetingRange { get; }
        public ITargetAbleEntity CurrentTarget { get;}
        public bool HaveTarget { get; }
        public IPriorityTargeting PriorityTargeting { get; }
        IEnumerable<ITargetAbleEntity> AvailableTargets { get; }
        public float GetDistanceToTarget(ITargetAbleEntity targetAbleEntity);
        public bool TrySetNewTarget(IPriorityTargeting priorityTargeting = null);
        public void SetAttackTarget(ITargetAbleEntity target);
    }
    
    [Flags]
    public enum EntityType
    {
        None = 0,
        Hero = 1,
        Enemy = 2,
        Structure = 4,
        Core = 8
    }
}