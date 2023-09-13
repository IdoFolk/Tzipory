using System;
using System.Collections.Generic;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.Tools.Interface;
using Tzipory.WaveSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.AbilitySystem.AbilityEntity.UI.WaveIndicator
{
    public class WaveIndicatorHandler : IInitialization<Wave,ITimer> , IDisposable
    {
        private readonly List<WaveIndicator> _waveIndicators;
        
        public bool IsInitialization { get; private set; }

        public WaveIndicatorHandler(Transform parent,int numberOfWaveSpawners)
        {
            _waveIndicators = new List<WaveIndicator>();

            for (int i = 0; i < numberOfWaveSpawners; i++)
            {
                var waveIndicator = PoolManager.IndicatorPool.GetObject();
                waveIndicator.transform.SetParent(parent);
                _waveIndicators.Add(waveIndicator);
            }
        }
        
        public void Init(Wave wave, ITimer timer)
        {
            var waveSpawners = wave.GetActiveWaveSpawners();

            foreach (var waveSpawner in waveSpawners)
            {
                foreach (var waveIndicator in _waveIndicators)
                {
                    if (waveIndicator.IsInitialization)
                        continue;
                    
                    waveIndicator.Init(waveSpawner,timer);
                    break;
                }
            }
            
            IsInitialization = true;
        }


        public void Dispose()
        {
            foreach (var waveIndicator in _waveIndicators)
                waveIndicator.Dispose();

            IsInitialization = false;
        }
    }
}