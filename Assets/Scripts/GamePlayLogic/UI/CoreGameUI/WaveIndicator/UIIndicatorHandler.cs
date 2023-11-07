using System;
using System.Collections.Generic;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.UISystem.Indicators;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UI.Indicator
{
    public class UIIndicatorHandler :  IDisposable
    {
        private static readonly List<UIIndicator> WaveIndicators;
        
        public bool IsInitialization { get; private set; }
        
        public UIIndicatorHandler(Transform parent,int numberOfWaveSpawners)
        {
            for (int i = 0; i < numberOfWaveSpawners; i++)
            {
                var waveIndicator = PoolManager.IndicatorPool.GetObject();
                waveIndicator.transform.SetParent(parent);
                WaveIndicators.Add(waveIndicator);
            }
        }

        public static IEnumerable<IDisposable> SetNewIndicator(Transform objectTransform, UIIndicatorConfig config, Action onCompleted = null)
        {
            List<IDisposable> indicators = new List<IDisposable>();
            
            foreach (var waveIndicator in WaveIndicators)
            {
                if (waveIndicator.IsInitialization)
                    continue;
                    
                waveIndicator.Init(objectTransform,config,onCompleted);
                indicators.Add(waveIndicator);
                break;
            }

            return indicators;
        }
        
        public static IEnumerable<IDisposable> SetNewIndicator(Transform objectTransform, UIIndicatorConfig config, ITimer timer)
        {
            List<IDisposable> indicators = new List<IDisposable>();
            
            foreach (var waveIndicator in WaveIndicators)
            {
                if (waveIndicator.IsInitialization)
                    continue;
                    
                waveIndicator.Init(objectTransform,config,timer);
                indicators.Add(waveIndicator);
                break;
            }

            return indicators;
        }
        
        public void Dispose()
        {
            foreach (var waveIndicator in WaveIndicators)
                waveIndicator.Dispose();

            IsInitialization = false;
        }
    }
}