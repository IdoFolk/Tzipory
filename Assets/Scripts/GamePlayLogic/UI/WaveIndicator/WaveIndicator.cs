using System;
using Helpers;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.Systems.PoolSystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlayLogic.UI.WaveIndicator
{
    public class WaveIndicator : MonoBehaviour , IInitialization<WaveSpawner,ITimer> , IPoolable<WaveIndicator>
    {
        public event Action<WaveIndicator> OnDispose;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _rotateIndicator;
        [SerializeField] private Image _timerFill;
        
        public bool IsInitialization { get; private set; }
        
        public void Init(WaveSpawner waveSpawner,ITimer timer)
        {
            var transformPosition = waveSpawner.transform.position;
            
            _rectTransform.SetScreenPointRelativeToWordPoint(transformPosition);
            _timerFill.fillAmount = 0;

            Vector3 dir = transformPosition - transform.position;
            
            _rotateIndicator.rotation = Quaternion.Euler(dir);
            gameObject.SetActive(true);
            
            IsInitialization  = true;
        }

        public void Dispose()
        {
            OnDispose?.Invoke(this);
            IsInitialization = false;
            gameObject.SetActive(false);
        }

        public void Free()
        {
            Destroy(gameObject);
        }
    }
}