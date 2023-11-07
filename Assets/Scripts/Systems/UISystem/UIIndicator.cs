using System;
using Tzipory.Helpers;
using Tzipory.Systems.PoolSystem;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tzipory.Systems.UISystem.Indicators
{
    public class UIIndicator : BaseInteractiveUIElement , IInitialization<Transform,UIIndicatorConfig,Action> , IInitialization<Transform,UIIndicatorConfig,ITimer>, IPoolable<UIIndicator>
    {
        public event Action<UIIndicator> OnDispose;
        
        [SerializeField] private GameObject _objectVisual;
        [SerializeField] private RectTransform _rotateIndicator;
        [SerializeField] private Image _bg;
        [SerializeField] private Image _timerFill;
        
        private Transform _transform;
        private UIIndicatorConfig _config;
        
        private ITimer _timer;
        
        private Action _onCompleted;

        private float _delay;

        private bool _isFlashing;

        private float _flashingTime;
        
        private float _flashingSpeed;
        
        private Vector2 _flashSize;
        private Vector2 _baseSize;
        
        public void Init(Transform objectTransform, UIIndicatorConfig config, ITimer timer)
        {
            _transform = objectTransform;
            _config = config;
            _timer = timer;
            _isFlashing = config.StartFlashing;
            
            _bg.sprite  = config.Image;
            _bg.color = config.Color;
            
            _delay = timer.TimeRemaining;
            
            _timerFill.gameObject.SetActive(true);
            
            _timerFill.fillAmount = 0;

            base.Init();
        }
        
        public void Init(Transform objectTransform, UIIndicatorConfig config, Action onCompleted = null)
        {
            _transform = objectTransform;
            _config = config;
            _onCompleted = onCompleted;
            _isFlashing = config.StartFlashing;
            
            _bg.sprite  = config.Image;
            _bg.color = config.Color;
            
            _timerFill.gameObject.SetActive(false);
            
            base.Init();
        }

        private void Update()
        {
            if (!IsInitialization)
                return;

            if (!_transform.InVisibleOnScreen())
            {
                _objectVisual.SetActive(false);
                return;
            }

            if(!_objectVisual.activeSelf)
                _objectVisual.SetActive(true);

            if (_timer is not null)
                _timerFill.fillAmount = _timer.TimeRemaining / _delay;
            
            var screenPoint = RectTransform.SetScreenPointRelativeToWordPoint(_transform.position,_config.OffSetRadios);

            if (_isFlashing)
            {
                _flashingTime -= GAME_TIME.GameDeltaTime;
                
                float lerpDelta = Mathf.PingPong(Time.time * _flashingSpeed, 1);
                
                RectTransform.sizeDelta = Vector2.Lerp(_baseSize,_flashSize,lerpDelta);
                _bg.color = Color.Lerp(_config.Color,_config.FlashingColor,lerpDelta);
                
                if (_flashingTime <= 0)
                {
                    _isFlashing = false;
                    RectTransform.sizeDelta = _baseSize;
                }
            }
            
            Vector3 dir = (new Vector3(screenPoint.x,screenPoint.y,0) - new Vector3(transform.position.x,transform.position.y,0)).normalized;
            float angle = Vector3.Angle(Vector3.up, dir);
            Vector3 newDir = new Vector3(0, 0, angle);
            _rotateIndicator.localEulerAngles = newDir;
        }

        public Action AddFlash(float sizeFactor,float flashSpeed,float time)
        {
            var sizeDelta = RectTransform.sizeDelta;
            
            _flashSize = new Vector2(sizeDelta.x * sizeFactor,sizeDelta.y * sizeFactor);
            _baseSize = sizeDelta;
            
            _flashingTime = time;
            
            _flashingSpeed = flashSpeed;
            
            _isFlashing = true;

            return CancelFlashing;
        }

        private void CancelFlashing()=>
            _flashingTime = 0;

        protected override void OnClick(PointerEventData eventData)
        {
            base.OnClick(eventData);
            _onCompleted?.Invoke();
            _timer?.StopTimer();
        }

        public void Dispose()
        {
            OnDispose?.Invoke(this);
            Hide();
            IsInitialization = false;
        }

        public void Free()
        {
            
        }
    }
}