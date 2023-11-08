using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.ConfigFiles.Level;
using Tzipory.GameplayLogic.UI.Indicator;
using Tzipory.Helpers;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.WaveSystem;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.Managers.CoreGameManagers
{
    public class WaveManager : IDisposable
    {
        public event Action<int> OnNewWaveStarted;
        
        private readonly LevelConfig _levelConfig;
        private readonly List<Wave> _waves;
        
        private float _levelStartDelay;
        private float _delayBetweenWaves;

#if UNITY_EDITOR
        private bool _isReportEndLevel;
#endif
        
        private int _currentWaveIndex;

        private ITimer _delayBetweenWavesTimer;
        private ITimer _startLevelTimer;

        private IEnumerable<IDisposable> _waveIndicators;
        
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
            _currentWaveIndex = 0;
            _levelStartDelay = _levelConfig.LevelStartDelay;
            _delayBetweenWaves = _levelConfig.DelayBetweenWaves;
        }
        
        public void StartLevel()
        {
            foreach (var waveSerialize in _levelConfig.Waves)
                _waves.Add(new Wave(_levelConfig.Level.WaveSpawners,waveSerialize));
            
#if UNITY_EDITOR
            _isReportEndLevel = false;
#endif
            
            _startLevelTimer = GAME_TIME.TimerHandler.StartNewTimer(_levelStartDelay,"Start Level Timer");
            CurrentWave.Init();
            
            var waveSpawners = CurrentWave.GetActiveWaveSpawners();
            List<Transform> waveIndicatorPositions = new List<Transform>();
            
            foreach (var waveSpawner in waveSpawners)
                waveIndicatorPositions.Add(waveSpawner.WaveIndicatorPosition);
                
            _waveIndicators = UIIndicatorHandler.SetNewIndicators(waveIndicatorPositions,_levelConfig.UIIndicatorConfig,_startLevelTimer);
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
                Debug.Log($"<color={ColorLogHelper.WAVE_MANAGER_COLOR}>WaveManager:</color> start wave-{_currentWaveIndex + 1}");
#endif
                CurrentWave.StartWave();

                foreach (var indicator in _waveIndicators)
                    indicator.Dispose();
                
                
                OnNewWaveStarted?.Invoke(_currentWaveIndex + 1);
            }

            if (!CurrentWave.IsAllWaveSpawnersDone) return;

            if (CurrentWave.IsActive && !CurrentWave.IsComplete)
            {
                CurrentWave.EndWave();
                Debug.Log($"<color={ColorLogHelper.WAVE_MANAGER_COLOR}>WaveManager:</color> ended wave-{_currentWaveIndex + 1}");
            }
            
            if (_currentWaveIndex + 1 < _waves.Count)
                _currentWaveIndex++;
            else
            {
#if UNITY_EDITOR
                if (!_isReportEndLevel)
                {
                    _isReportEndLevel = true;
                    Debug.Log($"<color={ColorLogHelper.WAVE_MANAGER_COLOR}>WaveManager:</color> <color=#f20505>Level Ended</color>");
                }
#endif
                return; // End level
            }
            
            CurrentWave.Init();
            
            _delayBetweenWavesTimer = GAME_TIME.TimerHandler.StartNewTimer(_delayBetweenWaves,"Delay Between Waves Timer");
            
            var waveSpawners = CurrentWave.GetActiveWaveSpawners();
            List<Transform> waveIndicatorPositions = new List<Transform>();
            
            foreach (var waveSpawner in waveSpawners)
                waveIndicatorPositions.Add(waveSpawner.WaveIndicatorPosition);
                
            _waveIndicators = UIIndicatorHandler.SetNewIndicators(waveIndicatorPositions,_levelConfig.UIIndicatorConfig,_startLevelTimer);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}