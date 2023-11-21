using System;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityTargetAbleComponent : IEntityHealthComponent , IEntityStatComponent
    {
        public event Action<IEntityTargetAbleComponent> OnTargetDisable;
        public bool IsTargetAble { get; }
        public EntityType EntityType { get; }
    }
    
    [Flags]
    public enum EntityType
    {
        None = 0,
        Hero = 1,
        Enemy = 2,
        Structure = 4,
        Core = 8,
        Totem = 16
    }
}