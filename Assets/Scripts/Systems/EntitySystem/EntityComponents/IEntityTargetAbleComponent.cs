using System;

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityTargetAbleComponent : IEntityHealthComponent , IEntityStatusEffectComponent
    {
        public event Action<IEntityTargetAbleComponent> OnTargetDisable;
        public bool IsTargetAble { get; }
        public EntityType EntityType { get; }
    }

    public enum EntityType
    {
        Hero,
        Enemy,
        Structure
    }
}