
using Tzipory.GameplayLogic.StatusEffectTypes;

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityStatusEffectComponent : IEntityComponent , IStatHolder
    {
        public StatusHandler StatusHandler { get; }
    }
}