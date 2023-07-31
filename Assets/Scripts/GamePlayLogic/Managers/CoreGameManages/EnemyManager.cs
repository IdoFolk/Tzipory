using System;
using System.Collections.Generic;
using Enemes;
using Tzipory.GamePlayLogic.ObjectPools;

public class EnemyManager : IDisposable
{
    private readonly List<Enemy> _enemies;

    public bool AllEnemiesArDead => _enemies.Count == 0;

    public int NumberOfEnemiesKilled { get; private set; }
   
    public EnemyManager()
    {
        NumberOfEnemiesKilled = 0;
        _enemies = new List<Enemy>();
        PoolManager.EnemyPool.OnObjectGet += AddEnemy;
    }

    public void AddEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
        enemy.OnDispose += OnEnemyKilled;
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        NumberOfEnemiesKilled++;
        _enemies.Remove(enemy);
    }

    public void Dispose()
    {
        PoolManager.EnemyPool.OnObjectGet -= AddEnemy;
        NumberOfEnemiesKilled  = 0;
    }
}