using System.Collections.Generic;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.Tools.Interface;
using Tzipory.WaveSystem;

namespace GamePlayLogic.UI.WaveIndicator
{
    public class WaveIndicatorHandler : IInitialization<Wave,ITimer>
    {
        private readonly List<WaveIndicator> _waveIndicators;
        
        public bool IsInitialization { get; private set; }

        public WaveIndicatorHandler(int numberOfWaveSpawners)
        {
            _waveIndicators = new List<WaveIndicator>();

            for (int i = 0; i < numberOfWaveSpawners; i++)
                _waveIndicators.Add(PoolManager.IndicatorPool.GetObject());
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
    }
}