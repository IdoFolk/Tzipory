using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.SerializeData.LevalSerializeData;
using Tzipory.WaveSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tzipory.Leval
{
    public class WaveManager : IDisposable
    {
        public event Action<int> OnNewWaveStarted;
        
        private readonly LevelConfig _levelConfig;
        private List<Wave> _waves;
        
        private float _levelStartDelay;
        private float _delayBetweenWaves;
        
        private int _currentWaveIndex;

        private ITimer _delayBetweenWavesTimer;

        public int WaveNumber => _currentWaveIndex + 1;

        public int TotalNumberOfWaves => _waves.Count;
        
        public  bool IsLastWave => _currentWaveIndex == _waves.Count - 1;

        public bool AllWaveAreDone => _waves.All(wave => wave.IsDone) && _currentWaveIndex  == _waves.Count - 1;
        
        private Wave CurrentWave => _waves[_currentWaveIndex];

        public WaveManager(LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
            _waves = new List<Wave>();
            _currentWaveIndex = 0;
            _levelStartDelay = _levelConfig.LevelStartDelay;
            _delayBetweenWaves = _levelConfig.DelayBetweenWaves;
        }
        
        public void StartLevel()
        {
            foreach (var waveSerialize in _levelConfig.Waves)
                _waves.Add(new Wave(_levelConfig.Level.WaveSpawners,waveSerialize));
        }

        public void UpdateLevel()
        {
            if (_levelStartDelay > 0)
            {
                _levelStartDelay -= GAME_TIME.GameDeltaTime;
                return;
            }

            if (!CurrentWave.IsStarted)
            {
#if UNITY_EDITOR
                Debug.Log($"<color=#2eff00>WaveManager:</color> start wave-{_currentWaveIndex + 1}");
#endif
                CurrentWave.StartWave();
                OnNewWaveStarted?.Invoke(_currentWaveIndex + 1);
            }

            if (!CurrentWave.IsDone) return;
            
            CurrentWave.EndWave();

            _delayBetweenWavesTimer ??= GAME_TIME.TimerHandler.StartNewTimer(_delayBetweenWaves);
                
            if (!_delayBetweenWavesTimer.IsDone)
                return;

            _delayBetweenWavesTimer = null;
            _delayBetweenWaves = _levelConfig.DelayBetweenWaves;
#if UNITY_EDITOR
            Debug.Log($"<color=#2eff00>WaveManager:</color> ended wave-{_currentWaveIndex + 1}");
#endif

            if (_currentWaveIndex + 1 < _waves.Count)
                _currentWaveIndex++;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}