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
    public class UIIndicator : BaseInteractiveUIElement , IInitialization<Transform,UIIndicatorConfig,Action> , IInitialization<Transform,UIIndicatorConfig,ITimer>, IPoolable<UIIndicator> , IObjectDisposable
    {
        public event Action<UIIndicator> OnDispose;
        
        [SerializeField] private GameObject _objectVisual;
        [SerializeField] private RectTransform _rotateIndicator;
        [SerializeField] private Image _bg;
        [SerializeField] private Image _timerFill;
        
        private Transform _objectTransform;
        private UIIndicatorConfig _config;
        
        private ITimer _timer;
        
        private Action _onCompleted;

        private float _delay;

        private bool _isFlashing;

        private float _flashingTime;
        
        public int ObjectInstanceId { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            ObjectInstanceId = InstanceIDGenerator.GetInstanceID();
            
            IsInitialization = false;
        }

        public override void Init()
        {
            base.Init();
            Show();
        }

        public void Init(Transform objectTransform, UIIndicatorConfig config, ITimer timer)
        {
            _objectTransform = objectTransform;
            _config = config;
            _timer = timer;
            
            if (config.StartFlashing)
            {
                var flashConfig = _config.FlashConfig;
                
                _isFlashing = true;
                
                if (flashConfig.UseTime)
                    _flashingTime = flashConfig.Time;
            }
            
            _bg.sprite  = config.Image;
            _bg.color = config.Color;
            
            _delay = timer.TimeRemaining;
            
            _timerFill.gameObject.SetActive(true);
            
            _timerFill.fillAmount = 0;

            Init();
        }
        
        public void Init(Transform objectTransform, UIIndicatorConfig config, Action onCompleted = null)
        {
            _objectTransform = objectTransform;
            _config = config;
            _onCompleted = onCompleted;

            if (config.StartFlashing)
            {
                var flashConfig = _config.FlashConfig;
                
                _isFlashing = true;
                
                if (flashConfig.UseTime)
                    _flashingTime = flashConfig.Time;
            }
            
            _bg.sprite  = config.Image;
            _bg.color = config.Color;
            
            _timerFill.gameObject.SetActive(false);
            
            Init();
        }

        private void Update()
        {
            if (!IsInitialization)
                return;

            if (!_config.AllwaysShow)
            {
                if (_objectTransform.InVisibleOnScreen())
                {
                    _objectVisual.SetActive(false);
                    return;
                }
                
                if(!_objectVisual.activeSelf)
                    _objectVisual.SetActive(true);
            }
            
            if (_timer is not null)
                _timerFill.fillAmount = _timer.TimeRemaining / _delay;
            
            var screenPoint = RectTransform.SetScreenPointRelativeToWordPoint(_objectTransform.position,_config.OffSetRadios);

            if (_isFlashing)
            {
                float lerpDelta = Mathf.PingPong(Time.time * _config.FlashConfig.FlashSpeed, 1);
                
                RectTransform.localScale = Vector2.Lerp(Vector2.one, Vector2.one * _config.FlashConfig.SizeFactor, lerpDelta);
                if (_config.FlashConfig.OverrideFlashingColor)
                    _bg.color = Color.Lerp(_config.Color,_config.FlashConfig.FlashingColor,lerpDelta);
                
                if (_config.FlashConfig.UseTime)
                {
                    _flashingTime -= GAME_TIME.GameDeltaTime;
                    
                    if (_flashingTime <= 0)
                        CancelFlashing();
                }
            }
            
            Vector3 dir = (new Vector3(screenPoint.x,screenPoint.y,0) - new Vector3(RectTransform.position.x,RectTransform.position.y,0)).normalized;
            float angle = Vector2Helper.AngleBetween360(Vector2.up, dir);
            Vector3 newDir = new Vector3(0, 0, angle);
            _rotateIndicator.localEulerAngles = newDir;
        }
        
        public Action StartFlash()
        {
            if (_isFlashing)
                return CancelFlashing;
            
            _isFlashing = true;

            return CancelFlashing;
        }
        
        public Action StartFlash(float time)
        {
            if (_isFlashing)
            {
                _flashingTime = time;
                return CancelFlashing;
            }
            
            _isFlashing = true;

            return CancelFlashing;
        }

        public Action StartFlash(UIIndicatorFlashConfig config)
        {
            if (_isFlashing)
            {
                _config.FlashConfig = config;
                _flashingTime = config.Time;
                return CancelFlashing;
            }
            
            _config.FlashConfig = config;
            
            _flashingTime = config.Time;
            
            _isFlashing = true;

            return CancelFlashing;
        }

        private void CancelFlashing()
        {
            _flashingTime = 0;
            _isFlashing = false;
            _bg.color = Color.white;
            RectTransform.localScale = Vector3.one;
        }

        protected override void OnClick(PointerEventData eventData)
        {
            base.OnClick(eventData);
            _onCompleted?.Invoke();
            _timer?.StopTimer();

            if (_isFlashing)
                CancelFlashing();
            
            if (_config.DisposOnClick)
                Dispose();
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