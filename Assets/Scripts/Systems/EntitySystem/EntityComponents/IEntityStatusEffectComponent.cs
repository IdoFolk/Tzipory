
using Tzipory.EntitySystem.StatusSystem;

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityStatusEffectComponent : IEntityComponent
    {
        public StatusHandler StatusHandler { get; }
        
        
    }
}