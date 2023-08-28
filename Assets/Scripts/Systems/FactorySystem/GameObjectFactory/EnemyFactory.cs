using Enemes;

namespace Systems.FactorySystem
{
    public class EnemyFactory : BaseGameObjectFactory<Enemy>
    {
        protected override string GameObjectPath => "Prefabs/Entities/Enemies/BaseEnemyEntity";
    }
}