using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.SerializeData.LevalSerializeData
{
    [CreateAssetMenu(fileName = "NewLevelConfig", menuName = "ScriptableObjects/New level config", order = 0)]
    public class LevelSerializeData : ScriptableObject
    {
        [SerializeField,PropertyOrder(-1)] private Level _level;
        [SerializeField,PropertyOrder(-1)] private Vector3 _fakeForwardVector;
        [SerializeField,PropertyOrder(-1)] private float _levelStartDelay = 0.2f;
        [SerializeField,PropertyOrder(-1)] private float _delayBetweenWaves;
        [SerializeField,PropertyOrder(1),ListDrawerSettings(HideAddButton = true,HideRemoveButton = true)] private List<WaveSerializeData> _waves;
        
        public Level Level => _level;
        public Vector3 FakeForwardVector => _fakeForwardVector;

        public List<WaveSerializeData> Waves => _waves;

        public float LevelStartDelay => _levelStartDelay;

        public float DelayBetweenWaves => _delayBetweenWaves;
        
        [Button("Add new wave"),PropertyOrder(0)]
        public void AddWave()
        {
            var waveData = new WaveSerializeData(_level.WaveSpawners,this);
            waveData.SetName($"Wave {_waves.Count + 1}"); 
            _waves.Add(waveData);
        }
        [Button("Reset data"),PropertyOrder(0)]
        public void ResetData()
        {
            _waves.Clear();
        }

        public void RemoveWave(WaveSerializeData waveSerializeData)
        {
            _waves.Remove(waveSerializeData);
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