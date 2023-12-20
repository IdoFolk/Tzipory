using System;
using System.Collections.Generic;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;

namespace Tzipory.GameplayLogic.Managers.CoreGameManagers
{
    public class EnemyManager : IDisposable
    {
        private static List<UnitEntity> _enemies;
        private static Transform _enemiesParent;

        public static bool AllEnemiesArDead => _enemies.Count == 0;

        public static int NumberOfEnemiesKilled { get; private set; }

        public EnemyManager(Transform enemiesParent)
        {
            _enemiesParent = enemiesParent;
            NumberOfEnemiesKilled = 0;
            _enemies = new List<UnitEntity>();
            //PoolManager.UnitEntityPool.OnObjectGet += AddEnemy;
        }

        public static void AddEnemy(UnitEntity enemy)
        {
            if (enemy.EntityType != EntityType.Enemy)
                throw  new Exception("Trying to add non-enemy to enemy manager");
            
            _enemies.Add(enemy);
            enemy.transform.SetParent(_enemiesParent);
            enemy.OnDispose += OnEnemyKilled;
        }

        private static void OnEnemyKilled(UnitEntity enemy)
        {
            NumberOfEnemiesKilled++;
            enemy.OnDispose -= OnEnemyKilled;
            _enemies.Remove(enemy);
        }

        public void Dispose()
        {
            //PoolManager.UnitEntityPool.OnObjectGet -= AddEnemy;
            NumberOfEnemiesKilled = 0;
            _enemies = null;
        }
    }
}