using System;
using System.Collections.Generic;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.GamePlayLogic.ObjectPools;
using UnityEngine;

namespace Tzipory.GameplayLogic.Managers.CoreGameManagers
{
    public class EnemyManager : IDisposable
    {
        private readonly List<UnitEntity> _enemies;
        private Transform _enemiesParent;

        public bool AllEnemiesArDead => _enemies.Count == 0;

        public int NumberOfEnemiesKilled { get; private set; }

        public EnemyManager(Transform enemiesParent)
        {
            _enemiesParent = enemiesParent;
            NumberOfEnemiesKilled = 0;
            _enemies = new List<UnitEntity>();
            PoolManager.UnitEntityPool.OnObjectGet += AddEnemy;
        }

        public void AddEnemy(UnitEntity enemy)
        {
            _enemies.Add(enemy);
            enemy.transform.SetParent(_enemiesParent);
            enemy.OnDispose += OnEnemyKilled;
        }

        private void OnEnemyKilled(UnitEntity enemy)
        {
            NumberOfEnemiesKilled++;
            _enemies.Remove(enemy);
        }

        public void Dispose()
        {
            PoolManager.UnitEntityPool.OnObjectGet -= AddEnemy;
            NumberOfEnemiesKilled = 0;
        }
    }
}