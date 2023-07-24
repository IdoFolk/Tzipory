using System;
using Enemes;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityConfigSystem;
using Tzipory.SerializeData.LevalSerializeData;

namespace Tzipory.WaveSystem
{
    public class EnemyGroup : WaveComponent<EnemyGroupConfig>
    {
        private EnemyConfig _enemyConfig;
        
        private int _spawnAmountPreInterval;

        private float _startDelay;

        private float _spawnInterval;
        public int TotalSpawnAmount { get; private set; }

        public override EnemyGroupConfig Data { get; }
        public override float CompletionPercentage => throw new NotImplementedException();
        public override bool IsDone => TotalSpawnAmount <= 0;
        
        public EnemyGroup(EnemyGroupConfig enemyGroupConfig)
        {
            Data = enemyGroupConfig;
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

                _spawnAmountPreInterval = Data.SpawnAmountPreInterval;
                _spawnInterval = Data.SpawnInterval;
            }
            else
                _spawnInterval -= GAME_TIME.GameDeltaTime;

            enemyPrefab = null;
            return false;
        }

    }
}


