
using Tzipory.Systems.StatusSystem;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityStatComponent : IEntityComponent , IStatHolder
    {
        public StatusHandler StatusHandler { get; }
    }
}