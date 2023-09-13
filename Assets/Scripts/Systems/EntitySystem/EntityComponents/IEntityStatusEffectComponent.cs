
using Tzipory.ConfigFiles.VisualSystemConfig;

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityStatusEffectComponent : IEntityComponent , IStatHolder
    {
        public StatusHandler StatusHandler { get; }
    }
}