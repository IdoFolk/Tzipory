using Tzipory.GamePlayLogic.EntitySystem;

namespace Tzipory.Systems.FactorySystem.GameObjectFactory
{
    public class UnitEntityFactory : BaseGameObjectFactory<UnitEntity>
    {
        protected override string GameObjectPath => "Prefabs/Entities/UnitEntity";
    }
}