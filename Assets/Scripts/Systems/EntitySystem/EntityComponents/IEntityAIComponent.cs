using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.Tools.Interface;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityAIComponent : IEntityComponent , IInitialization<BaseGameEntity,UnitEntity,AIComponentConfig>
    {
        
    }
}