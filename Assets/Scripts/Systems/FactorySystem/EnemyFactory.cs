using Enemes;
using Tzipory.Systems.FactorySystem;
using UnityEngine;

namespace Systems.FactorySystem
{
    public class EnemyFactory : IFactory<Enemy>
    {
        private const string  EnemyPath = "Prefabs/Entities/Enemies/BaseEnemyEntity";

        private readonly Enemy _enemy;

        public EnemyFactory()
        {
            _enemy = Resources.Load<Enemy>(EnemyPath);
        }
        
        public Enemy Create()
        {
           var enemy =  Object.Instantiate(_enemy);
           enemy.gameObject.SetActive(false);
           return enemy;
        }
    }
}