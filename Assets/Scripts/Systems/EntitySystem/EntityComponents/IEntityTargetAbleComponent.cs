

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityTargetAbleComponent : IEntityHealthComponent , IEntityStatusEffectComponent
    {
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