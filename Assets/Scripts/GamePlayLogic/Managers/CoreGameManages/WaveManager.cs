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
using Logger = Tzipory.Tools.Debag.Logger;

namespace Tzipory.GameplayLogic.Managers.CoreGameManagers
{
    public class WaveManager : IDisposable
    {
        public const string WAVE_MANAGER_LOG_GROUP = "WaveManager";
        public event Action<int> OnNewWaveStarted;
        
        private readonly LevelConfig _levelConfig;
        private readonly List<Wave> _waves;
        
        private readonly UIIndicatorConfig _uiIndicatorConfig;
        
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
        
        public WaveManager(LevelConfig levelConfig,UIIndicatorConfig uiIndicatorConfig)//temp UIIndicator config
        {
            _levelConfig = levelConfig;
            _waves = new List<Wave>();
            _uiIndicatorConfig = uiIndicatorConfig;
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
                
            _waveIndicators = UIIndicatorHandler.SetNewIndicators(waveIndicatorPositions,_uiIndicatorConfig,_startLevelTimer);
        }

        public void UpdateLevel()
        {
            if (!_startLevelTimer.IsDone)
                return;
            
            if (_delayBetweenWavesTimer is { IsDone: false })
                return;

            if (!CurrentWave.IsActive && !CurrentWave.IsComplete)
            {
                Logger.Log($"Start wave-{_currentWaveIndex + 1}",WAVE_MANAGER_LOG_GROUP);
                CurrentWave.StartWave();

                foreach (var indicator in _waveIndicators)
                    indicator.Dispose();
                
                
                OnNewWaveStarted?.Invoke(_currentWaveIndex + 1);
            }

            if (!CurrentWave.IsAllWaveSpawnersDone) return;

            if (CurrentWave.IsActive && !CurrentWave.IsComplete)
            {
                CurrentWave.EndWave();
                Logger.Log($"Ended wave-{_currentWaveIndex + 1}",WAVE_MANAGER_LOG_GROUP);
            }
            
            if (_currentWaveIndex + 1 < _waves.Count)
                _currentWaveIndex++;
            else
            {
#if UNITY_EDITOR
                if (!_isReportEndLevel)
                {
                    _isReportEndLevel = true;
                    Logger.Log($"<color=#f20505>Level Ended</color>",WAVE_MANAGER_LOG_GROUP);
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
                
            _waveIndicators = UIIndicatorHandler.SetNewIndicators(waveIndicatorPositions,_uiIndicatorConfig,_delayBetweenWavesTimer);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}