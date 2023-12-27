using Tzipory.GamePlayLogic.EntitySystem;
using UnityEngine;

namespace Tzipory.Systems.FactorySystem.GameObjectFactory
{
    public class EnemyFactory : BaseGameObjectFactory<Enemy>
    {
        protected override string GameObjectPath => "Prefabs/Entities/Units/Enemy";
        
    }
}