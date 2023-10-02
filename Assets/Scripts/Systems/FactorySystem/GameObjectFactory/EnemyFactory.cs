using Tzipory.GameplayLogic.EntitySystem.Enemies;

namespace Tzipory.Systems.FactorySystem.GameObjectFactory
{
    public class EnemyFactory : BaseGameObjectFactory<Enemy>
    {
        protected override string GameObjectPath => "Prefabs/Entities/Enemies/BaseEnemyEntity";
    }
}