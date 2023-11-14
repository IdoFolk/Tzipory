using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.UISystem.Indicators;
using Tzipory.Tools.Interface;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UI.Indicator
{
    public class UIIndicatorHandler :  IDisposable
    {
        private static readonly Dictionary<int,UIIndicator> UIIndicators = new();
        
        public bool IsInitialization { get; private set; }
        
        public UIIndicatorHandler(Transform parent,int numberOfUIIndicators)
        {
            for (int i = 0; i < numberOfUIIndicators; i++)
            {
                var uiIndicator = PoolManager.IndicatorPool.GetObject();
                uiIndicator.transform.SetParent(parent);
                UIIndicators.Add(uiIndicator.ObjectInstanceId,uiIndicator);
            }
        }

        public static IEnumerable<IObjectDisposable> SetNewIndicators(IEnumerable<Transform> objectsTransform, UIIndicatorConfig config,ITimer timer = null ,Action onCompleted = null)
        {
            int objectIndex = 0;
            
            List<IObjectDisposable> indicators = new List<IObjectDisposable>();

            var objectsTransformArray = objectsTransform.ToArray();
            
            foreach (var waveIndicator in UIIndicators.Values)
            {
                if (waveIndicator.IsInitialization)
                    continue;

                if (timer is null)
                     waveIndicator.Init(objectsTransformArray[objectIndex],config,onCompleted);
                else
                    waveIndicator.Init(objectsTransformArray[objectIndex],config,timer);
                
                indicators.Add(waveIndicator);
                objectIndex++;

                if (objectIndex >= objectsTransformArray.Length)
                    break;
            }

            return indicators;
        }
        
        public static IObjectDisposable SetNewIndicator(Transform objectTransform, UIIndicatorConfig config,ITimer timer = null , Action onCompleted = null)
        {
            foreach (var waveIndicator in UIIndicators.Values)
            {
                if (waveIndicator.IsInitialization)
                    continue;
                
                if (timer is null)
                    waveIndicator.Init(objectTransform,config,onCompleted);
                else
                    waveIndicator.Init(objectTransform,config,timer);
                    
                return waveIndicator;
            }

            return null;
        }

        public static Action StartFlashOnIndicator(int indicatorId, UIIndicatorFlashConfig config)
        {
            if (UIIndicators.TryGetValue(indicatorId, out var waveIndicator))
                return waveIndicator.StartFlash(config);
            Debug.LogWarning($"Can not find UIIndicator by id {indicatorId}");
            return null;
        }
        
        public static Action StartFlashOnIndicator(int indicatorId,float time)
        {
            if (UIIndicators.TryGetValue(indicatorId, out var waveIndicator))
                return waveIndicator.StartFlash(time);
            Debug.LogWarning($"Can not find UIIndicator by id {indicatorId}");
            return null;
        }
        
        public static Action StartFlashOnIndicator(int indicatorId)
        {
            if (UIIndicators.TryGetValue(indicatorId, out var waveIndicator))
                return waveIndicator.StartFlash();
            Debug.LogWarning($"Can not find UIIndicator by id {indicatorId}");
            return null;
        }
        
        public void Dispose()
        {
            foreach (var waveIndicator in UIIndicators.Values)
                waveIndicator.Dispose();

            UIIndicators.Clear();
            
            IsInitialization = false;
        }
    }
}