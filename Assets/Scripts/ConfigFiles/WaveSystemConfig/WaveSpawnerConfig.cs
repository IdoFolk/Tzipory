using Sirenix.OdinInspector;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.SerializeData.LevalSerializeData
{
    [System.Serializable]
    public class WaveSpawnerConfig
    {
        [SerializeField,ReadOnly] private int _id;
        
        [SerializeField,ReadOnly,PropertyOrder(-3)] private Color _waveSpawnerColor;
        [SerializeField,PropertyOrder(-2)] private float _delayBetweenEnemyGroup;
        [SerializeField,PropertyOrder(1)] private EnemyGroupConfig[] _enemyGroups;

        private float _startTime;
        
        [ShowInInspector,ReadOnly,PropertyOrder(-1)]
        public float TotalSpawnerTime
        {
            get
            {
                float  totalTime = 0;
                
                if (_enemyGroups == null)
                    return 0;

                foreach (var groupSerializeData in _enemyGroups)
                {
                    if (groupSerializeData == null)
                        continue;

                    if (groupSerializeData.EndTime > totalTime)
                        totalTime = groupSerializeData.EndTime - _startTime;
                }

                totalTime += _delayBetweenEnemyGroup * (_enemyGroups.Length - 1);
                
                return  totalTime;
            }
        }

        public int ID => _id;

        public EnemyGroupConfig[] EnemyGroups => _enemyGroups;

        public float DelayBetweenEnemyGroup => _delayBetweenEnemyGroup;

        public WaveSpawnerConfig(WaveSpawner waveSpawner)
        {
            _waveSpawnerColor = waveSpawner.WaveSpawnerColor;
           _id = waveSpawner.ID;
        }
        
        public void OnValidate(float startTime)
        {
            _startTime = startTime;
            
            float lastStartTime = _startTime;
            
            for (int i = 0; i < _enemyGroups.Length; i++)
            {
                if (i == 0)
                {
                    _enemyGroups[i].OnValidate(lastStartTime);
                    continue;
                }

                if (_enemyGroups[i].StartType == ActionStartType.AfterPrevious)
                    lastStartTime = _enemyGroups[i - 1].EndTime;

                _enemyGroups[i].OnValidate(lastStartTime);
            }
        }

        private Color GetColor()
            => _waveSpawnerColor;
    }
}