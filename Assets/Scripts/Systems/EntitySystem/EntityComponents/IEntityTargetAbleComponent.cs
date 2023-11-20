using System;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityTargetAbleComponent : IEntityHealthComponent , IEntityStatComponent
    {
        public event Action<IEntityTargetAbleComponent> OnTargetDisable;
        public bool IsTargetAble { get; }
        public EntityType EntityType { get; }
    }

    public enum EntityType
    {
        Hero,
        Enemy,
        Structure,
        Core,
        Totem
    }
}