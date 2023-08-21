using System;
using Tzipory.Systems.PoolSystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlayLogic.UI.WaveIndicator
{
    public class WaveIndicator : MonoBehaviour , IInitialization , IPoolable<WaveIndicator>
    {
        public event Action<WaveIndicator> OnDispose;
        
        [SerializeField] private RectTransform _rotateIndicator;
        [SerializeField] private Image _timerFill;
        
        public bool IsInitialization { get; private set; }
        
        public void Init()
        {
            _timerFill.fillAmount = 1;
            IsInitialization  = true;
        }

        public void Dispose()
        {
            OnDispose?.Invoke(this);
        }

        public void Free()
        {
            Destroy(gameObject);
        }
    }
}
