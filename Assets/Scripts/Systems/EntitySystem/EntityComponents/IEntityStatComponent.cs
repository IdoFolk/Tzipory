
using Tzipory.Systems.StatusSystem;

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityStatComponent : IEntityComponent , IStatHolder
    {
        public StatusHandler StatusHandler { get; }
    }
}