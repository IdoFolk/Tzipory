using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Systems.AbilitySystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityAbilitiesComponent : IEntityComponent,IInitialization<BaseGameEntity,AbilityComponentConfig> , IStatHolder
    {
        public AbilityHandler AbilityHandler { get; }
    }
}