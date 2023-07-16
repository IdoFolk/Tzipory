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
    public class LevelManager : IDisposable
    {
        public event Action<int> OnNewWaveStarted;
        
        private LevelSerializeData _levelSerializeData;
        private Transform _levelPerant;
        private List<Wave> _waves;
    
        private IEnumerable<WaveSpawner> _waveSpawners;

        private float _levelStartDelay;
        private float _delayBetweenWaves;
        
        private int _currentWaveIndex;

        private ITimer _delayBetweenWavesTimer;

        public int WaveNumber => _currentWaveIndex + 1;

        public int TotalNumberOfWaves => _waves.Count;
        
        public  bool IsLastWave => _currentWaveIndex == _waves.Count - 1;

        public bool AllWaveAreDone => _waves.All(wave => wave.IsDone);
        
        private Wave CurrentWave => _waves[_currentWaveIndex];

        public LevelManager(LevelSerializeData levelSerializeData,Transform levelPerant)
        {
            _levelPerant  = levelPerant;
            _levelSerializeData = levelSerializeData;
            
            _currentWaveIndex = 0;
            _levelStartDelay = _levelSerializeData.LevelStartDelay;
            _delayBetweenWaves = _levelSerializeData.DelayBetweenWaves;

            Object.Instantiate(_levelSerializeData.Level,_levelPerant);
            _waveSpawners = GameManager.GetWaveSpawners();//temppp
            
            _waves = new List<Wave>();
        
            foreach (var waveSerialize in _levelSerializeData.Waves)
                _waves.Add(new Wave(_waveSpawners,waveSerialize));
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
                CurrentWave.StartWave();
                OnNewWaveStarted?.Invoke(_currentWaveIndex + 1);
            }

            if (!CurrentWave.IsDone) return;
            
            _delayBetweenWavesTimer ??= GAME_TIME.TimerHandler.StartNewTimer(_delayBetweenWaves);
                
            if (!_delayBetweenWavesTimer.IsDone)
                return;

            _delayBetweenWavesTimer = null;
            _delayBetweenWaves = _levelSerializeData.DelayBetweenWaves;
            CurrentWave.EndWave();

            if (_currentWaveIndex + 1 < _waves.Count)
                _currentWaveIndex++;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}