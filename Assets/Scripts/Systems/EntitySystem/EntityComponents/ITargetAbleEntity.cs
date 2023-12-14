using System;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface ITargetAbleEntity : IEntity
    {
        public event Action<ITargetAbleEntity> OnTargetDisable;
        public bool IsTargetAble { get; }
        public EntityType EntityType { get; }

        public IEntityHealthComponent EntityHealthComponent { get; }
        public IEntityStatComponent EntityStatComponent { get;  }
        public IEntityVisualComponent EntityVisualComponent { get; }
    }
}