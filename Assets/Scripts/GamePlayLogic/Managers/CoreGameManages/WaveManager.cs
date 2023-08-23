using System;
using System.Collections.Generic;
using System.Linq;
using GamePlayLogic.UI.WaveIndicator;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.SerializeData.LevalSerializeData;
using Tzipory.WaveSystem;
using UnityEngine;

namespace Tzipory.Leval
{
    public class WaveManager : IDisposable
    {
        public event Action<int> OnNewWaveStarted;
        
        private readonly LevelConfig _levelConfig;
        private readonly List<Wave> _waves;
        private readonly WaveIndicatorHandler _waveIndicatorHandler;
        
        
        private float _levelStartDelay;
        private float _delayBetweenWaves;
        
        private int _currentWaveIndex;

        private ITimer _delayBetweenWavesTimer;
        private ITimer _startLevelTimer;
        
        #region Proprty

        public int WaveNumber => _currentWaveIndex + 1;

        public int TotalNumberOfWaves => _waves.Count;
        
        public  bool IsLastWave => _currentWaveIndex == _waves.Count - 1;

        public bool AllWaveAreDone => _waves.All(wave => wave.IsDone) && _currentWaveIndex  == _waves.Count - 1;
        
        private Wave CurrentWave => _waves[_currentWaveIndex];

        private Wave NextWave => _currentWaveIndex + 1 >= _waves.Count ? null : _waves[_currentWaveIndex + 1];

        #endregion
        
        public WaveManager(LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
            _waves = new List<Wave>();
            _waveIndicatorHandler = new WaveIndicatorHandler(_levelConfig.Level.WaveSpawners.Count());//need to check the count fun
            _currentWaveIndex = 0;
            _levelStartDelay = _levelConfig.LevelStartDelay;
            _delayBetweenWaves = _levelConfig.DelayBetweenWaves;
        }
        
        public void StartLevel()
        {
            foreach (var waveSerialize in _levelConfig.Waves)
                _waves.Add(new Wave(_levelConfig.Level.WaveSpawners,waveSerialize));
            
            _startLevelTimer = GAME_TIME.TimerHandler.StartNewTimer(_levelStartDelay);
            CurrentWave.Init();
            _waveIndicatorHandler.Init(CurrentWave,_startLevelTimer);
        }

        public void UpdateLevel()
        {
            if (!_startLevelTimer.IsDone)
                return;

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
            NextWave.Init();

            _delayBetweenWavesTimer ??= GAME_TIME.TimerHandler.StartNewTimer(_delayBetweenWaves);
            
            _waveIndicatorHandler.Init(NextWave,_delayBetweenWavesTimer);
            
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