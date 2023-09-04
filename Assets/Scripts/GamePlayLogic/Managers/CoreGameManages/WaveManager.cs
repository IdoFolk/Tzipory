using System;
using System.Collections.Generic;
using System.Linq;
using GamePlayLogic.Managers;
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

#if UNITY_EDITOR
        private bool _isReportEndLevel;
#endif
        
        private int _currentWaveIndex;

        private ITimer _delayBetweenWavesTimer;
        private ITimer _startLevelTimer;
        
        #region Proprty

        public int WaveNumber => _currentWaveIndex + 1;

        public int TotalNumberOfWaves => _waves.Count;
        
        public  bool IsLastWave => _currentWaveIndex == _waves.Count - 1;

        public bool AllWaveAreDone => _waves.All(wave => wave.IsComplete) && _currentWaveIndex  == _waves.Count - 1;
        
        private Wave CurrentWave => _waves[_currentWaveIndex];

        private Wave NextWave => _currentWaveIndex + 1 >= _waves.Count ? null : _waves[_currentWaveIndex + 1];

        #endregion
        
        public WaveManager(LevelConfig levelConfig,Transform waveIndicatorParent)
        {
            _levelConfig = levelConfig;
            _waves = new List<Wave>();
            _waveIndicatorHandler = new WaveIndicatorHandler(waveIndicatorParent,_levelConfig.Level.WaveSpawners.Count());//need to check the count fun
            _currentWaveIndex = 0;
            _levelStartDelay = _levelConfig.LevelStartDelay;
            _delayBetweenWaves = _levelConfig.DelayBetweenWaves;
        }
        
        public void StartLevel()
        {
            foreach (var waveSerialize in _levelConfig.Waves)
                _waves.Add(new Wave(_levelConfig.Level.WaveSpawners,waveSerialize));

            _isReportEndLevel = false;
            
            _startLevelTimer = GAME_TIME.TimerHandler.StartNewTimer(_levelStartDelay);
            CurrentWave.Init();
            _waveIndicatorHandler.Init(CurrentWave,_startLevelTimer);
        }

        public void UpdateLevel()
        {
            if (!_startLevelTimer.IsDone)
                return;
            
            if (_delayBetweenWavesTimer is { IsDone: false })
                return;

            if (!CurrentWave.IsActive && !CurrentWave.IsComplete)
            {
#if UNITY_EDITOR
                Debug.Log($"<color=#2eff00>WaveManager:</color> start wave-{_currentWaveIndex + 1}");
#endif
                CurrentWave.StartWave();
                _waveIndicatorHandler.Dispose();
                OnNewWaveStarted?.Invoke(_currentWaveIndex + 1);
            }

            if (!CurrentWave.IsAllWaveSpawnersDone) return;

            if (CurrentWave.IsActive && !CurrentWave.IsComplete)
            {
                CurrentWave.EndWave();
                Debug.Log($"<color=#2eff00>WaveManager:</color> ended wave-{_currentWaveIndex + 1}");
            }
            
            if (_currentWaveIndex + 1 < _waves.Count)
                _currentWaveIndex++;
            else
            {
#if UNITY_EDITOR
                if (!_isReportEndLevel)
                {
                    _isReportEndLevel = true;
                    Debug.Log($"<color=#2eff00>WaveManager:</color> <color=#f20505>Level Ended</color>");
                }
#endif
                return; // End level
            }
            
            CurrentWave.Init();
            
            _delayBetweenWavesTimer = GAME_TIME.TimerHandler.StartNewTimer(_delayBetweenWaves);
            
            _waveIndicatorHandler.Init(CurrentWave,_delayBetweenWavesTimer);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}