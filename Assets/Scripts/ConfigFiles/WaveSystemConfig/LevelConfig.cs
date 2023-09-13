using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.ConfigFiles.WaveSystemConfig
{
    [CreateAssetMenu(fileName = "NewLevelConfig", menuName = "ScriptableObjects/New level config", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField,PropertyOrder(-2)] private int _levelId;
        [SerializeField,PropertyOrder(-1)] private Level _level;
        [SerializeField,PropertyOrder(-1)] private float _levelStartDelay = 0.2f;
        [SerializeField,PropertyOrder(-1)] private float _delayBetweenWaves;
        [SerializeField,PropertyOrder(1),ListDrawerSettings(HideAddButton = true,HideRemoveButton = true)] private List<WaveConfig> _waves;

        public int LevelId => _levelId;

        public Level Level => _level;
        
        public List<WaveConfig> Waves => _waves;

        public float LevelStartDelay => _levelStartDelay;

        public float DelayBetweenWaves => _delayBetweenWaves;
        
        [Button("Add new wave"),PropertyOrder(0)]
        public void AddWave()
        {
            var waveData = new WaveConfig(_level.WaveSpawnersSerialize,this);
            waveData.SetName($"Wave {_waves.Count + 1}"); 
            _waves.Add(waveData);
        }
        [Button("reset data"),PropertyOrder(0)]
        public void ResetData()
        {
            _waves.Clear();
        }

        public void RemoveWave(WaveConfig waveConfig)
        {
            _waves.Remove(waveConfig);
            OnValidate();
        }

        private void OnValidate()
        {
            float lastStartTime = _levelStartDelay;
            
            for (int i = 0; i < _waves.Count; i++)
            {
                _waves[i].SetName($"Wave {i + 1}");
                
                _waves[i].OnValidate(lastStartTime);
                
                lastStartTime += _delayBetweenWaves + _waves[i].TotalWaveTime;
            }
        }
    }
}