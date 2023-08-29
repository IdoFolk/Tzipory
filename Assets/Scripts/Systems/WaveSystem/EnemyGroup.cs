using System;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.SerializeData.LevalSerializeData;
using Tzipory.Tools.Interface;

namespace Tzipory.WaveSystem
{
    public class EnemyGroup : IProgress
    {
        private readonly EnemyConfig _enemyConfig;
        private readonly EnemyGroupConfig _data;
        
        private int _spawnAmountPreInterval;

        private float _startDelay;

        private float _spawnInterval;
        
        
        public int TotalSpawnAmount { get; private set; }

        public float CompletionPercentage => throw new NotImplementedException();
        public bool IsDone => TotalSpawnAmount <= 0;
        
        public EnemyGroup(EnemyGroupConfig enemyGroupConfig)
        {
            _data = enemyGroupConfig;
            _startDelay = enemyGroupConfig.GroupStartDelay;
            _enemyConfig = enemyGroupConfig.EnemyConfig;
            TotalSpawnAmount = enemyGroupConfig.TotalSpawnAmount;
            _spawnInterval = 0;
            _spawnAmountPreInterval = enemyGroupConfig.SpawnAmountPreInterval;
        }

        public bool TryGetEnemy(out EnemyConfig enemyPrefab)
        {
            if (TotalSpawnAmount <= 0)
            {
                enemyPrefab = null;
                return false;
            }

            if (_startDelay > 0)
            {
                _startDelay  -= GAME_TIME.GameDeltaTime;
                enemyPrefab = null;
                return false;
            }

            if (_spawnInterval <= 0)
            {
                while (_spawnAmountPreInterval > 0)
                {
                    _spawnAmountPreInterval--; 
                    TotalSpawnAmount--;
                    enemyPrefab = _enemyConfig;
                    return true;
                }

                _spawnAmountPreInterval = _data.SpawnAmountPreInterval;
                _spawnInterval = _data.SpawnInterval;
            }
            else
                _spawnInterval -= GAME_TIME.GameDeltaTime;

            enemyPrefab = null;
            return false;
        }

    }
}


