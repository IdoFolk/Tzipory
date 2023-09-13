using System;
using Helpers;
using Systems.UISystem;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.GameplayLogic.Managers.MainGameManagers;
using Tzipory.Systems.PoolSystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Tzipory.GameplayLogic.AbilitySystem.AbilityEntity.UI.WaveIndicator
{
    public class WaveIndicator : BaseUIElement , IInitialization<WaveSpawner,ITimer> , IPoolable<WaveIndicator>
    {
        public event Action<WaveIndicator> OnDispose;
        
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _rotateIndicator;
        
        [SerializeField] private Vector2 _offSet;
        
        [SerializeField] private Image _timerFill;

        private WaveSpawner _waveSpawner;

        private ITimer _timer;

        private float _delay;
        
        public bool IsInitialization { get; private set; }

        private void Awake()
        {
            OnClick += Click;
        }

        public void Init(WaveSpawner waveSpawner,ITimer timer)
        {
            _waveSpawner = waveSpawner;
            _timer = timer;
            
            _delay = timer.TimeRemaining;
            
            _timerFill.fillAmount = 0;
            
            gameObject.SetActive(true);
            
            IsInitialization  = true;
        }

        private void Update()
        {
            if (!IsInitialization)
                return;
            
            var transformPosition = _waveSpawner.WaveIndicatorPosition.transform.position;
            
            _rectTransform.SetScreenPointRelativeToWordPoint(transformPosition,_offSet);

            _timerFill.fillAmount = _timer.TimeRemaining / _delay;

            var waveSpawnerScreenPoint = GameManager.Camera.WorldToScreenPoint(_waveSpawner.transform.position);
            
            Vector3 dir = (waveSpawnerScreenPoint - transform.position).normalized;
            float angle = Vector3.Angle(Vector3.up, dir);
            Vector3 newDir = new Vector3(0, 0, angle);
            _rotateIndicator.localEulerAngles = newDir;
        }

        private void Click()
        {
            _timer.StopTimer();
            Debug.Log("Click");
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

        private void OnDestroy()
        {
            OnClick += Click;
        }

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            
            Vector3 pos = new Vector3(position.x + _offSet.x, position.y + _offSet.y, 0);
            Gizmos.DrawSphere(pos,5);
        }
    }
}